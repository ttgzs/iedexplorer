/* Mkulu Ini File Manager
 * Copyright (c) 2010 Chad Ullman
 *
 * This software is provided 'as-is', without any express or implied warranty. In
 * no event will the authors be held liable for any damages arising from the use
 * of this software.
 *
 * Permission is granted to anyone to use this software for any purpose, including
 * commercial applications, and to alter it and redistribute it freely, subject to
 * the following restrictions:
 *
 *    1. The origin of this software must not be misrepresented; you must not claim
 *       that you wrote the original software. If you use this software in a product,
 *       an acknowledgment in the product documentation would be appreciated but is
 *       not required.
 *
 *    2. Altered source versions must be plainly marked as such, and must not be
 *       misrepresented as being the original software.
 *
 *    3. This notice may not be removed or altered from any source distribution.
 *
 *
 * This class provides a simple interface to config data stored in the now classic Microsoft .ini file
 * format. Originally the class used the Win32 Get/WritePrivateProfileString functions, but that was later
 * changed to the current implementation, where the class parses the config file itself.  Blank lines are
 * ignored, as are any lines starting with a semi-colon (;) or REM.  The class supports encrypting the config file.
 *
 * Any comments in the config file will be lost once it has been processed by this class.
 *
 * The config file is saved to disk every time you use one of the writeXXX functions.  While this is somewhat
 * inefficient, realistically we are not going to be writing out thosands of config entries all the time, and in
 * real world use the benefits of never having to worry about flushing the updates to disk outweigh any nano-second
 * performance benefits we might get by implemeting a transaction/commit system.
 *
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using System.Diagnostics;

namespace org.mkulu.config {
    #region IniFileManager
    public class IniFileManager {
        #region Data Members
        private string configFileName;
        private byte[] cryptoKey;
        private byte[] cryptoInitializationVector;
        private bool fileIsEncrypted;
        private AesManaged aes;
        private SortedDictionary<string, StringDictionary> cache = new SortedDictionary<string, StringDictionary>(new IniCacheSorter<string>());
        #endregion

        #region Constructors
        public IniFileManager(string fileToManage) {
            configFileName = fileToManage;
            fileIsEncrypted = false;
            cacheIniFile();
        }

        public IniFileManager(string fileToManage, string key) {
            configFileName = fileToManage;
            fileIsEncrypted = true;
            aes = new AesManaged();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            cryptoKey = ASCIIEncoding.ASCII.GetBytes(key);
            cryptoInitializationVector = cryptoKey;
            if (cryptoInitializationVector.Length != (aes.BlockSize / 8)) {
                Array.Resize(ref cryptoInitializationVector, (aes.BlockSize / 8));
            }
            if (cryptoKey.Length != (aes.KeySize / 8)) {
                Array.Resize(ref cryptoKey, (aes.KeySize / 8));
            }

            cacheIniFile();
        }
        #endregion

        #region Encryption Utilities (from http://remy.supertext.ch/2011/01/simple-c-encryption-and-decryption/)
        private string Encrypt(string source) {
            if (String.IsNullOrEmpty(source)) {
                return "";
            }

            ICryptoTransform encryptor = aes.CreateEncryptor(cryptoKey, cryptoInitializationVector);

            try {
                byte[] sourceBytes = ASCIIEncoding.ASCII.GetBytes(source);
                byte[] encryptedSource = encryptor.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length);
                return Convert.ToBase64String(encryptedSource);
            } catch (CryptographicException cex) {
                throw new Exception("Unable to encrypt ini file: " + cex.Message, cex);
            } catch (Exception) {
                throw;
            }
        }

        private string Decrypt(string source) {
            if (String.IsNullOrEmpty(source)) {
                return "";
            }

            ICryptoTransform decryptor = aes.CreateDecryptor(cryptoKey, cryptoInitializationVector);

            try {
                byte[] encryptedSourceBytes = Convert.FromBase64String(source);
                byte[] sourceBytes = decryptor.TransformFinalBlock(encryptedSourceBytes, 0, encryptedSourceBytes.Length);
                return ASCIIEncoding.ASCII.GetString(sourceBytes);
            } catch (CryptographicException cex) {
                throw new Exception("Unable to decrypt ini file: " + cex.Message, cex);
            } catch (Exception) {
                throw;
            }
        }
        #endregion

        #region Cache Handling
        private void cacheIniFile() {
            string configData;

            if (!File.Exists(configFileName)) {
                return;
            }

            configData = File.ReadAllText(configFileName);

            //It is always possible to pass in an unencrypted ini file which we want to encrypt, so we need to check for that
            int encryptionIndicator = configData.IndexOf(Environment.NewLine);
            if (encryptionIndicator == -1 && !fileIsEncrypted) {
                throw new Exception("Config file is encrypted, but no key provided");
            }
            if (encryptionIndicator == -1 && fileIsEncrypted) {
                configData = Decrypt(configData);
            }

            string[] lines = configData.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

            StringDictionary sectionData = null;
            string sectionName = "";
            Match res;
            foreach (string line in lines) {
                Debug.WriteLine("Processing line " + line);
                if (Regex.IsMatch(line, @"^\s*;") || Regex.IsMatch(line, @"^\s*rem\s+", RegexOptions.IgnoreCase)) { //comment line, skip
                    Debug.WriteLine("  line is a comment, skipping");
                    continue;
                } else if (Regex.IsMatch(line, @"^\[.*\]")) { //section header
                    Debug.WriteLine("  line is a section header, processing");
                    if (sectionData != null) {
                        cache.Add(sectionName, sectionData);
                    }
                    res = Regex.Match(line, @"\[(.*)\]");
                    sectionName = res.Groups[1].Value;
                    //sectionName = sectionName.ToLower();
                    Debug.WriteLine("  sectionName = " + sectionName);
                    sectionData = new StringDictionary();
                } else {
                    Debug.WriteLine("  line may be section data");
                    Match parts = Regex.Match(line, @"^([^=]*)=(.*)");
                    if (parts.Groups.Count != 3 || sectionData == null) {
                        Debug.WriteLine("  nope, this is in fact not section data");
                        continue;
                    }
                    Debug.WriteLine("  " + parts.Groups[1].Value.Trim() + "=" + parts.Groups[2].Value.Trim());
                    sectionData.Add(parts.Groups[1].Value.Trim(), parts.Groups[2].Value.Trim());
                }
            }
            // the last section we processed won't have been saved yet, so do that now
            cache.Add(sectionName, sectionData);
        }

        private void dumpCache() {
            StringBuilder configData = new StringBuilder();

            foreach (KeyValuePair<string, StringDictionary> section in cache) {
                configData.AppendLine("[" + section.Key + "]");
                foreach (DictionaryEntry data in section.Value) {
                    configData.AppendLine(String.Format("{0}={1}", data.Key, data.Value));
                }
                configData.AppendLine();
            }

            string config = configData.ToString();

            //dump cache in to configData
            if (fileIsEncrypted) {
                config = Encrypt(config);
            }

            File.WriteAllText(configFileName, config);
        }
        #endregion

        #region Data Access - Read
        public string getString(string section, string key, string defaultValue) {
            key = key.ToLower();
            if (!cache.ContainsKey(section)) {
                return defaultValue;
            }
            StringDictionary sectionData = (StringDictionary)cache[section];
            if (!sectionData.ContainsKey(key)) {
                return defaultValue;
            }

            return sectionData[key];
        }

        public int getInt(string section, string key, int defaultValue) {
            string value = getString(section, key, defaultValue.ToString());
            int result;
            try {
                result = Convert.ToInt32(value);
                return result;
            } catch {
                return defaultValue;
            }
        }

        public bool getBool(string section, string key, bool defaultValue) {
            string value = getString(section, key, defaultValue.ToString());
            bool result;
            if (Boolean.TryParse(value, out result)) {
                return result;
            } else {
                return defaultValue;
            }
        }

        public List<string> getSections() {
            return cache.Keys.ToList<string>();
        }

        public StringDictionary getSection(string section) {
            if (cache.Keys.Contains(section)) {
                return cache[section];
            } else {
                return null;
            }
        }
        #endregion

        #region Data Access - Write
        public void writeString(string section, string key, string value) {
            if (!cache.ContainsKey(section)) {
                cache.Add(section, new StringDictionary());
            }

            StringDictionary sectionData = (StringDictionary)cache[section];
            if (!sectionData.ContainsKey(key)) {
                sectionData.Add(key, value);
            } else {
                sectionData[key] = value;
            }
            dumpCache();
        }

        public void writeInt(string section, string key, int value) {
            writeString(section, key, value.ToString());
        }

        public void writeBool(string section, string key, bool value) {
            writeString(section, key, value.ToString());
        }
        #endregion
        
        #region Data Access - Delete
        public void deleteSection(string section) {
            if (cache.ContainsKey(section)) {
        		cache.Remove(section);
        		dumpCache();
            }        	
        }
        
        public void deleteKey(string section, string key) {
        	if (cache.ContainsKey(section) && ((StringDictionary)cache[section]).ContainsKey(key)) {
        		((StringDictionary)(cache[section])).Remove(key);
        		dumpCache();
        	}
        }
        #endregion
    }
    #endregion

    #region IniCacheSorter
    public class IniCacheSorter<T> : IComparer<T> where T: IComparable<T> {

        // Return values
        //    Less than zero: x is less than y.
        //              Zero: x equals y.
        // Greater than zero: x is greater than y.
        public int Compare(T x, T y) {
            string first  = x as string;
            string second = y as string;

            return string.Compare(first, second, true); //case insensitive comparison
        }
    }
    #endregion
}
