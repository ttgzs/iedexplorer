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
using System.Reflection;
using System.Collections.Generic;
using org.bn.utils;
using org.bn.attributes;
using org.bn.metadata;
using org.bn.types;
using org.bn.coders;
using System.IO;

namespace IEDExplorer.BNExtension
{
	
	public class XMLEncoder: Encoder
	{		
		public XMLEncoder()
		{
		}

        private int indent = 0;
        private int indOfs = 4;
        private int unknown = 0;

        public override void encode<T>(T obj, System.IO.Stream stream)
		{
            base.encode(obj, stream);
		}

        int printString(System.IO.Stream stream, String str)
        {
            byte[] buf = System.Text.Encoding.ASCII.GetBytes(str);
            for (int i=0; i < indent; i++) stream.WriteByte(32);
            stream.Write(buf,0,buf.Length);
            return buf.Length;
        }

        public override int encodeSequence(object obj, System.IO.Stream stream, ElementInfo elementInfo)
		{
			int resultSize = 0;
            string s = "";
            if (elementInfo.hasPreparedInfo() && elementInfo.hasPreparedASN1ElementInfo())
            {
                s = elementInfo.PreparedASN1ElementInfo.Name;
            }
            if (s == "") 
            {
                s = "unnamedSequence" + unknown.ToString("D3");
                unknown++;
            }
            resultSize += printString(stream, "<" + s + " type=\"Sequence\">\r\n");
            indent += indOfs;
            PropertyInfo[] fields = elementInfo.getProperties(obj.GetType());
            for (int i = 0; i < fields.Length; i++)
			{
                PropertyInfo field = fields[fields.Length - 1 - i];
                resultSize += encodeSequenceField(obj, fields.Length - 1 - i, field, stream, elementInfo);
			}

            indent -= indOfs;
            resultSize += printString(stream, "</" + s + ">\r\n");
            return resultSize;
		}

        public override int encodeChoice(object obj, System.IO.Stream stream, ElementInfo elementInfo)
		{
			int result = 0;
            string s = "";
            if (elementInfo.hasPreparedInfo() && elementInfo.hasPreparedASN1ElementInfo())
            {
                s = elementInfo.PreparedASN1ElementInfo.Name;
            }
            else
            {
                s = elementInfo.AnnotatedClass.ToString();
                string[] sa = s.Split(new char[1] { '.' });
                s = sa[sa.Length - 1];
                //indent = 0;
                //unknown = 0;
            }
            result += printString(stream, "<" + s + " type=\"Choice\">\r\n");
            indent += indOfs;
            int sizeOfChoiceField = base.encodeChoice(obj, stream, elementInfo);
			result += sizeOfChoiceField;
            indent -= indOfs;
            result += printString(stream, "</" + s + ">\r\n");
            return result;
		}

        public override int encodeEnumItem(object enumConstant, Type enumClass, System.IO.Stream stream, ElementInfo elementInfo)
		{
			int resultSize = 0;
            ASN1EnumItem enumObj = elementInfo.getAttribute<ASN1EnumItem>();
            string s = "";
            if (elementInfo.hasPreparedInfo() && elementInfo.hasPreparedASN1ElementInfo())
            {
                s = elementInfo.PreparedASN1ElementInfo.Name;
            }
            if (s == "")
            {
                s = "unnamedEnum" + unknown.ToString("D3");
                unknown++;
            }
            resultSize += printString(stream, "<" + s + " type=\"Enum\">" + enumObj.Tag.ToString() + "</" + s + ">\r\n");
			return resultSize;
		}

        public override int encodeBoolean(object obj, System.IO.Stream stream, ElementInfo elementInfo)
		{
			int resultSize = 1;
            bool value = (bool)obj;
            string s = "";
            if (elementInfo.hasPreparedInfo() && elementInfo.hasPreparedASN1ElementInfo())
            {
                s = elementInfo.PreparedASN1ElementInfo.Name;
            }
            if (s == "")
            {
                s = "unnamedBoolean" + unknown.ToString("D3");
                unknown++;
            }
            resultSize += printString(stream, "<" + s + " type=\"Boolean\">" + value.ToString() + "</" + s + ">\r\n");
            return resultSize;
		}

        public override int encodeAny(object obj, System.IO.Stream stream, ElementInfo elementInfo)
		{
			int resultSize = 0, sizeOfString = 0;
			byte[] buffer = (byte[]) obj;
            CoderUtils.checkConstraints(sizeOfString, elementInfo);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                sb.Append(buffer[i]);
                sb.Append(',');
            }
            string s = "";
            if (elementInfo.hasPreparedInfo() && elementInfo.hasPreparedASN1ElementInfo())
            {
                s = elementInfo.PreparedASN1ElementInfo.Name;
            }
            if (s == "")
            {
                s = "unnamedAny" + unknown.ToString("D3");
                unknown++;
            }
            resultSize += printString(stream, "<" + s + " type=\"Any\">" + sb.ToString() + "</" + s + ">\r\n");
			return resultSize;
		}
		
        public override int encodeInteger(object obj, System.IO.Stream stream, ElementInfo elementInfo)
		{
			int resultSize = 0;
            string s = "";
            if (elementInfo.hasPreparedInfo() && elementInfo.hasPreparedASN1ElementInfo())
            {
                s = elementInfo.PreparedASN1ElementInfo.Name;
            }
            if (s == "")
            {
                s = "unnamedInteger" + unknown.ToString("D3");
                unknown++;
            }
            resultSize += printString(stream, "<" + s + " type=\"Integer\">" + obj.ToString() + "</" + s + ">\r\n");
            return resultSize;
		}

        public override int encodeReal(object obj, System.IO.Stream stream, ElementInfo elementInfo)
        {
            int resultSize = 0;
            Double value = (Double) obj;
            string s = "";
            if (elementInfo.hasPreparedInfo() && elementInfo.hasPreparedASN1ElementInfo())
            {
                s = elementInfo.PreparedASN1ElementInfo.Name;
            }
            if (s == "")
            {
                s = "unnamedReal" + unknown.ToString("D3");
                unknown++;
            }
            resultSize += printString(stream, "<" + s + " type=\"Real\">" + value.ToString() + "</" + s + ">\r\n");
            return resultSize;
        }

        public override int encodeOctetString(object obj, System.IO.Stream stream, ElementInfo elementInfo)
		{
			int resultSize = 0, sizeOfString = 0;
			byte[] buffer = (byte[]) obj;
            sizeOfString = buffer.Length;
            string s = "";
            if (elementInfo.hasPreparedInfo() && elementInfo.hasPreparedASN1ElementInfo())
            {
                s = elementInfo.PreparedASN1ElementInfo.Name;
            }
            if (s == "")
            {
                s = "unnamedOctetString" + unknown.ToString("D3");
                unknown++;
            }
            resultSize += printString(stream, "<" + s + " type=\"OctetString\">");
            stream.Write(buffer, 0, sizeOfString);
            resultSize += sizeOfString;
            resultSize += printString(stream, "</" + s + ">\r\n");
            return resultSize;
		}

        public override int encodeString(object obj, System.IO.Stream stream, ElementInfo elementInfo)
		{
			int resultSize = 0;//, sizeOfString = 0;
            string s = "";
            if (elementInfo.hasPreparedInfo() && elementInfo.hasPreparedASN1ElementInfo())
            {
                s = elementInfo.PreparedASN1ElementInfo.Name;
            }
            if (s == "")
            {
                s = "unnamedString" + unknown.ToString("D3");
                unknown++;
            }
            resultSize += printString(stream, "<" + s + " type=\"String\">" + (string)obj + "</" + s + ">\r\n");
            return resultSize;
		}

        public override int encodeSequenceOf(object obj, System.IO.Stream stream, ElementInfo elementInfo)
		{
			int resultSize = 0;
            string s = "";
            if (elementInfo.hasPreparedInfo() && elementInfo.hasPreparedASN1ElementInfo())
            {
                s = elementInfo.PreparedASN1ElementInfo.Name;
            }
            if (s == "")
            {
                s = "unnamedSequenceOf" + unknown.ToString("D3");
                unknown++;
            }
            resultSize += printString(stream, "<" + s + " type=\"SequenceOf\">\r\n");
            indent += indOfs;
            System.Collections.IList collection = (System.Collections.IList)obj;
            CoderUtils.checkConstraints(collection.Count, elementInfo);
            
            int sizeOfCollection = 0;
			for (int i = 0; i < collection.Count; i++)
			{
				object item = collection[collection.Count - 1 - i];
				ElementInfo info = new ElementInfo();
                info.AnnotatedClass = item.GetType();
                info.ParentAnnotatedClass = elementInfo.AnnotatedClass;

                if (elementInfo.hasPreparedInfo() && elementInfo.PreparedInfo.TypeMetadata != null)
                {
                    ASN1SequenceOfMetadata seqOfMeta = (ASN1SequenceOfMetadata)elementInfo.PreparedInfo.TypeMetadata;
                    info.PreparedInfo = (seqOfMeta.getItemClassMetadata());
                }

				sizeOfCollection += encodeClassType(item, stream, info);
			}
			resultSize += sizeOfCollection;
            indent -= indOfs;
            resultSize += printString(stream, "</" + s + ">\r\n");
			return resultSize;
		}
		
        public override int encodeNull(object obj, System.IO.Stream stream, ElementInfo elementInfo)
		{
			int resultSize = 0;

            string s = "";
            if (elementInfo.hasPreparedInfo() && elementInfo.hasPreparedASN1ElementInfo())
            {
                s = elementInfo.PreparedASN1ElementInfo.Name;
            }
            if (s == "")
            {
                s = "unnamedNull" + unknown.ToString("D3");
                unknown++;
            }
            resultSize += printString(stream, "<" + s + " type=\"Null\"/>\r\n");
            return resultSize;
		}

        public override int encodeBitString(Object obj, System.IO.Stream stream, ElementInfo elementInfo)
        {
            int resultSize = 0;
            BitString str = (BitString)obj;
            CoderUtils.checkConstraints(str.getLengthInBits(), elementInfo);
            byte[] buffer = str.Value;
            string s = "";
            if (elementInfo.hasPreparedInfo() && elementInfo.hasPreparedASN1ElementInfo())
            {
                s = elementInfo.PreparedASN1ElementInfo.Name;
            }
            if (s == "")
            {
                s = "unnamedBitString" + unknown.ToString("D3");
                unknown++;
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                sb.Append(buffer[i]);
                sb.Append(',');
            }
            resultSize += printString(stream, "<" + s + " type=\"BitString\">" + sb.ToString() + "</" + s + ">\r\n");
            return resultSize;
        }

        public override int encodeObjectIdentifier(Object obj, System.IO.Stream stream, ElementInfo elementInfo)
        {
            ObjectIdentifier oid = (ObjectIdentifier)obj;
            string s = "";
            if (elementInfo.hasPreparedInfo() && elementInfo.hasPreparedASN1ElementInfo())
            {
                s = elementInfo.PreparedASN1ElementInfo.Name;
            }
            if (s == "")
            {
                s = "unnamedObjectIdentifier" + unknown.ToString("D3");
                unknown++;
            }
            return printString(stream, "<" + s + " type=\"ObjectIdentifier\">" + oid.Value + "</" + s + ">\r\n");
        }
	}
}
