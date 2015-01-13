/*
 *  Copyright (C) 2013 Pavel Charvat
 * 
 *  This file is part of IEDExplorer.
 *
 *  IEDExplorer is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  IEDExplorer is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with IEDExplorer.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.IO;

namespace IEDExplorer
{
    public class Logger
    {
        public enum Severity
        {
            Debug,
            Information,
            Warning,
            Error
        }

        string tmpFile;
        Stream stream;
        TextWriter writer;
        Severity verbosity;

        static Logger sLog;
        public static Logger getLogger()
        {
            if (sLog == null)
                sLog = new Logger();
            return sLog;
        }

        private Logger()
        {
            verbosity = Severity.Information;
            //verbosity = Severity.Debug;
            try
            {
                tmpFile = /*Path.Combine(Path.GetTempPath(),*/ "MMS_log_file.txt";
                stream = new FileStream(tmpFile, FileMode.Create, FileAccess.Write, FileShare.Read);
                writer = new StreamWriter(stream);
            }
            catch { }
        }

        ~Logger()
        {
            if (stream != null) stream.Close();
        }

        public virtual void Log(Severity severity, string message)
        {
            //Console.WriteLine(string.Format("{0}: {1}", severity.ToString(), message));

            try
            {
                string msg = string.Format("[{0}.{1}] {2}: {3}", DateTime.Now, DateTime.Now.Millisecond.ToString("D3"), severity.ToString(), message);
                writer.WriteLine(msg);
                writer.Flush();
                if (OnLogMessage != null)
                    OnLogMessage(msg); //, null, null);
            }
            catch { }
        }

        public void LogDebug(string message)
        {
            if (verbosity == Severity.Debug)
                Log(Severity.Debug, message);
        }

        public void LogDebugBuffer(string message, byte[] buffer, long logFrom, long logLength)
        {
            if (verbosity == Severity.Debug)
            {
                string s = message + " (Len=" + logLength + ")>";
                for (long i = logFrom; i < logFrom + logLength; i++)
                    s += String.Format("{0:x2} ", buffer[i]);
                Log(Severity.Debug, s);
            }
        }

        public void LogInfo(string message)
        {
            if (verbosity <= Severity.Information)
                Log(Severity.Information, message);
        }

        public void LogWarning(string message)
        {
            if (verbosity <= Severity.Warning)
                Log(Severity.Warning, message);
        }

        public void LogError(string message)
        {
            Log(Severity.Error, message);
        }

         public void ClearLog()
        {
            if (OnClearLog != null)
                OnClearLog();
        }

       public Severity Verbosity
        {
            get { return verbosity; }
            set { verbosity = value; Log(Severity.Information, "Verbosity selected: " + verbosity.ToString()); }
        }

        public delegate void OnLogMessageDelegate(string message);
        public event OnLogMessageDelegate OnLogMessage;

        public delegate void OnClearLogDelegate();
        public event OnClearLogDelegate OnClearLog;
    }
}
