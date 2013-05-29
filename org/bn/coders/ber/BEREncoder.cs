/*
* Copyright 2006 Abdulla G. Abdurakhmanov (abdulla.abdurakhmanov@gmail.com).
* 
* Licensed under the LGPL, Version 2 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
* 
*      http://www.gnu.org/copyleft/lgpl.html
* 
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
* 
* With any your questions welcome to my e-mail 
* or blog at http://abdulla-a.blogspot.com.
*/
using System;
using System.Reflection;
using System.Collections.Generic;
using org.bn.utils;
using org.bn.attributes;
using org.bn.metadata;
using org.bn.types;

namespace org.bn.coders.ber
{
	
	public class BEREncoder: Encoder
	{		
		public BEREncoder()
		{
		}

        public override void encode<T>(T obj, System.IO.Stream stream)
		{
			ReverseByteArrayOutputStream reverseStream = new ReverseByteArrayOutputStream();
			base.encode(obj, reverseStream);
			reverseStream.WriteTo(stream);
		}

        public override int encodeSequence(object obj, System.IO.Stream stream, ElementInfo elementInfo)
		{
			int resultSize = 0;
            PropertyInfo[] fields = elementInfo.getProperties(obj.GetType());
            for (int i = 0; i < fields.Length; i++)
			{
                PropertyInfo field = fields[fields.Length - 1 - i];
                resultSize += encodeSequenceField(obj, fields.Length - 1 - i, field, stream, elementInfo);
			}

            if(!CoderUtils.isSequenceSet(elementInfo)) 
            {
                resultSize += encodeHeader(
                    BERCoderUtils.getTagValueForElement(
                        elementInfo,
                        TagClasses.Universal,
                        ElementType.Constructed,
                        UniversalTags.Sequence)
                    , resultSize, stream);
            }
            else
            {
                resultSize += encodeHeader(
                    BERCoderUtils.getTagValueForElement(
                        elementInfo,
                        TagClasses.Universal,
                        ElementType.Constructed,
                        UniversalTags.Set)
                    , resultSize, stream);
            }
			return resultSize;
		}

        public override int encodeChoice(object obj, System.IO.Stream stream, ElementInfo elementInfo)
		{
			int result = 0;
			int sizeOfChoiceField = base.encodeChoice(obj, stream, elementInfo);
            if (
                (elementInfo.hasPreparedInfo() && elementInfo.hasPreparedASN1ElementInfo() && elementInfo.PreparedASN1ElementInfo.HasTag )
                || ( elementInfo.ASN1ElementInfo != null && elementInfo.ASN1ElementInfo.HasTag) )
            {
                result += encodeHeader(BERCoderUtils.getTagValueForElement(elementInfo, TagClasses.ContextSpecific, ElementType.Constructed, UniversalTags.LastUniversal), sizeOfChoiceField, stream);
            }
			result += sizeOfChoiceField;
			return result;
		}

        public override int encodeEnumItem(object enumConstant, Type enumClass, System.IO.Stream stream, ElementInfo elementInfo)
		{
			int resultSize = 0;
            ASN1EnumItem enumObj = //elementInfo.AnnotatedClass.getAnnotation(typeof(ASN1EnumItem));
                elementInfo.getAttribute<ASN1EnumItem>();
			int szOfInt = encodeIntegerValue(enumObj.Tag, stream);
			resultSize += szOfInt;
			resultSize += encodeLength(szOfInt, stream);
			resultSize += encodeTag(BERCoderUtils.getTagValueForElement(elementInfo, TagClasses.Universal, ElementType.Primitive, UniversalTags.Enumerated), stream);
			return resultSize;
		}

        public override int encodeBoolean(object obj, System.IO.Stream stream, ElementInfo elementInfo)
		{
			int resultSize = 1;
            bool value = (bool)obj;
			stream.WriteByte((byte) (value ? 0xFF:0x00));
			
			resultSize += encodeLength(1, stream);
			resultSize += encodeTag(BERCoderUtils.getTagValueForElement(elementInfo, TagClasses.Universal, ElementType.Primitive, UniversalTags.Boolean), stream);
			return resultSize;
		}

        public override int encodeAny(object obj, System.IO.Stream stream, ElementInfo elementInfo)
		{
			int resultSize = 0, sizeOfString = 0;
			byte[] buffer = (byte[]) obj;
            sizeOfString = buffer.Length;
            CoderUtils.checkConstraints(sizeOfString, elementInfo);
            stream.Write(buffer, 0, buffer.Length);
			resultSize += sizeOfString;
			return resultSize;
		}
		
		protected internal int encodeIntegerValue(long val, System.IO.Stream stream)
		{            
			int resultSize = CoderUtils.getIntegerLength(val);
			for (int i = 0; i < resultSize; i++)
			{
				stream.WriteByte((byte) val);
				val = val >> 8;
			}
			return resultSize;
		}

        public override int encodeInteger(object obj, System.IO.Stream stream, ElementInfo elementInfo)
		{
			int resultSize = 0;
            int szOfInt = 0;
            if (obj.GetType().Equals(typeof(int)))
            {
                int val = (int)obj;
                CoderUtils.checkConstraints(val, elementInfo);
                szOfInt = encodeIntegerValue(val, stream);
            }
            else
            {
                long val = (long)obj;
                CoderUtils.checkConstraints(val, elementInfo);
                szOfInt = encodeIntegerValue(val, stream);
            }
			resultSize += szOfInt;
			resultSize += encodeLength(szOfInt, stream);
			resultSize += encodeTag(BERCoderUtils.getTagValueForElement(elementInfo, TagClasses.Universal, ElementType.Primitive, UniversalTags.Integer), stream);
			return resultSize;
		}

        public override int encodeReal(object obj, System.IO.Stream stream, ElementInfo elementInfo)
        {
            int resultSize = 0;
            Double value = (Double) obj;
            //CoderUtils.checkConstraints(value,elementInfo);
            int szOfInt = 0;
#if PocketPC
            byte[] dblValAsBytes =  System.BitConverter.GetBytes(value);
            long asLong = System.BitConverter.ToInt64(dblValAsBytes, 0);
#else            
            long asLong = System.BitConverter.DoubleToInt64Bits(value);
#endif
            if (value == Double.PositiveInfinity)
            { // positive infinity
                stream.WriteByte(0x40); // 01000000 Value is PLUS-INFINITY
            }
            else
            if(value == Double.NegativeInfinity) 
            { // negative infinity            
                stream.WriteByte(0x41); // 01000001 Value is MINUS-INFINITY
            }        
            else 
            if(asLong!=0x0) {
                long exponent = ((0x7ff0000000000000L & asLong) >> 52) - 1023 - 52;
                long mantissa = 0x000fffffffffffffL & asLong;
                mantissa |= 0x10000000000000L; // set virtual delimeter
                
                // pack mantissa for base 2
                while((mantissa & 0xFFL) == 0x0) {
                    mantissa >>= 8;
                    exponent += 8; //increment exponent to 8 (base 2)
                }        
                while((mantissa & 0x01L) == 0x0) {
                    mantissa >>= 1;
                    exponent+=1; //increment exponent to 1
                }
                 
                 szOfInt+= encodeIntegerValue(mantissa,stream);
                 int szOfExp = CoderUtils.getIntegerLength(exponent);
                 szOfInt+= encodeIntegerValue(exponent,stream);
                 
                 byte realPreamble = 0x80;
                 realPreamble |= (byte)(szOfExp - 1);
                 if( ((ulong)asLong & 0x8000000000000000L) == 1) {
                     realPreamble|= 0x40; // Sign
                 }
                 stream.WriteByte(realPreamble );
                 szOfInt+=1;
            }
            resultSize += szOfInt;
            resultSize += encodeLength(szOfInt, stream);
            resultSize += encodeTag(BERCoderUtils.getTagValueForElement(elementInfo, TagClasses.Universal, ElementType.Primitive, UniversalTags.Real), stream);
            return resultSize;
        }

        public override int encodeOctetString(object obj, System.IO.Stream stream, ElementInfo elementInfo)
		{
			int resultSize = 0, sizeOfString = 0;
			byte[] buffer = (byte[]) obj;
            sizeOfString = buffer.Length;
            CoderUtils.checkConstraints(sizeOfString, elementInfo);

			stream.Write(buffer, 0, buffer.Length);
			
			resultSize += sizeOfString;
			resultSize += encodeLength(sizeOfString, stream);
			resultSize += encodeTag(BERCoderUtils.getTagValueForElement(elementInfo, TagClasses.Universal, ElementType.Primitive, UniversalTags.OctetString), stream);
			return resultSize;
		}

        public override int encodeString(object obj, System.IO.Stream stream, ElementInfo elementInfo)
		{
			int resultSize = 0, sizeOfString = 0;
            byte[] buffer = CoderUtils.ASN1StringToBuffer(obj, elementInfo); 
            sizeOfString = buffer.Length;
            CoderUtils.checkConstraints(sizeOfString, elementInfo);

            stream.Write(buffer, 0, buffer.Length);
			
			resultSize += sizeOfString;
			resultSize += encodeLength(sizeOfString, stream);
			resultSize += encodeTag(BERCoderUtils.getTagValueForElement(elementInfo, TagClasses.Universal, ElementType.Primitive, CoderUtils.getStringTagForElement(elementInfo)), stream);
			return resultSize;
		}

        public override int encodeSequenceOf(object obj, System.IO.Stream stream, ElementInfo elementInfo)
		{
			int resultSize = 0;
            System.Collections.IList collection = (System.Collections.IList)obj;
            CoderUtils.checkConstraints(collection.Count, elementInfo);
            
            int sizeOfCollection = 0;
			for (int i = 0; i < collection.Count; i++)
			{
				object item = collection[collection.Count - 1 - i];
				ElementInfo info = new ElementInfo();
                info.AnnotatedClass = item.GetType();
                info.ParentAnnotatedClass = elementInfo.AnnotatedClass;

                if (elementInfo.hasPreparedInfo())
                {
                    ASN1SequenceOfMetadata seqOfMeta = (ASN1SequenceOfMetadata)elementInfo.PreparedInfo.TypeMetadata;
                    info.PreparedInfo = (seqOfMeta.getItemClassMetadata());
                }

				sizeOfCollection += encodeClassType(item, stream, info);
			}
			resultSize += sizeOfCollection;
			resultSize += encodeLength(sizeOfCollection, stream);

            if(!CoderUtils.isSequenceSetOf(elementInfo))
            {
                resultSize += encodeTag(BERCoderUtils.getTagValueForElement(elementInfo, TagClasses.Universal, ElementType.Constructed, UniversalTags.Sequence), stream);
            }
            else
            {
                resultSize += encodeTag(BERCoderUtils.getTagValueForElement(elementInfo, TagClasses.Universal, ElementType.Constructed, UniversalTags.Set), stream);
            }
			return resultSize;
		}
		
		protected internal int encodeHeader(DecodedObject<int> tagValue, int contentLen, System.IO.Stream stream)
		{
			int resultSize = encodeLength(contentLen, stream);
			resultSize += encodeTag(tagValue, stream);
			return resultSize;
		}

        protected internal int encodeTag(DecodedObject<int> tagValue, System.IO.Stream stream)
		{
            int resultSize = tagValue.Size;
            int value = tagValue.Value;
            for (int i = 0; i < tagValue.Size; i++)
            {
                stream.WriteByte((byte)value);
                value = value >> 8;
            }
            return resultSize;

            /*int resultSize = 0;
            if (tagValue.Size == 1)
            {
                stream.WriteByte((byte)tagValue.Value);
                resultSize++;
            }
            else
                resultSize += encodeIntegerValue(tagValue.Value, stream);
            return resultSize;*/
		}
		
		protected internal int encodeLength(int length, System.IO.Stream stream)
		{
            return BERCoderUtils.encodeLength (length, stream);
		}

        public override int encodeNull(object obj, System.IO.Stream stream, ElementInfo elementInfo)
		{
			stream.WriteByte((byte) 0);
			int resultSize = 1;
			resultSize += encodeTag(BERCoderUtils.getTagValueForElement(elementInfo, TagClasses.Universal, ElementType.Primitive, UniversalTags.Null), stream);
			return resultSize;
		}

        public override int encodeBitString(Object obj, System.IO.Stream stream, ElementInfo elementInfo)
        {
            int resultSize = 0, sizeOfString = 0;
            BitString str = (BitString)obj;
            CoderUtils.checkConstraints(str.getLengthInBits(), elementInfo);
            byte[] buffer = str.Value;
            stream.Write( buffer, 0, buffer.Length );
            stream.WriteByte ( (byte) str.getTrailBitsCnt() );
            sizeOfString = buffer.Length + 1;
            
            resultSize += sizeOfString;
            resultSize += encodeLength(sizeOfString, stream);
            resultSize += encodeTag( 
                BERCoderUtils.getTagValueForElement(elementInfo,TagClasses.Universal, ElementType.Primitive, UniversalTags.Bitstring),
                stream
            );
            return resultSize;
        }

        public override int encodeObjectIdentifier(Object obj, System.IO.Stream stream, ElementInfo elementInfo)
        {
            ObjectIdentifier oid = (ObjectIdentifier)obj;
            int[] ia = oid.getIntArray();
            byte[] buffer = BERObjectIdentifier.Encode(ia);
            stream.Write(buffer, 0, buffer.Length);
            int resultSize = buffer.Length;
            resultSize += encodeLength(resultSize, stream);
            resultSize += encodeTag(
                BERCoderUtils.getTagValueForElement(elementInfo, TagClasses.Universal, ElementType.Primitive, UniversalTags.ObjectIdentifier),
                stream
            );
            return resultSize;
        }
	}
}
