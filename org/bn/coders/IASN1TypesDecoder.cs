/*
 * Copyright 2007 Abdulla G. Abdurakhmanov (abdulla.abdurakhmanov@gmail.com).
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
using System.IO;
using org.bn.attributes;

namespace org.bn.coders
{
    public interface IASN1TypesDecoder {
        DecodedObject<object> decodeTag(Stream stream)  ;
        DecodedObject<object> decodeClassType(DecodedObject<object> decodedTag, Type objectClass, ElementInfo elementInfo, Stream stream) ;
        DecodedObject<object> decodeSequence(DecodedObject<object> decodedTag, Type objectClass, ElementInfo elementInfo, Stream stream) ;
        DecodedObject<object> decodeChoice(DecodedObject<object> decodedTag,Type objectClass, ElementInfo elementInfo, Stream stream)  ;    
        DecodedObject<object> decodeBoolean(DecodedObject<object> decodedTag, Type objectClass, ElementInfo elementInfo, Stream stream) ;
        DecodedObject<object> decodeAny(DecodedObject<object> decodedTag, Type objectClass, ElementInfo elementInfo, Stream stream)  ;
        DecodedObject<object> decodeNull(DecodedObject<object> decodedTag, Type objectClass, ElementInfo elementInfo, Stream stream)  ;
        DecodedObject<object> decodeInteger(DecodedObject<object> decodedTag, Type objectClass, ElementInfo elementInfo, Stream stream)  ;
        DecodedObject<object> decodeReal(DecodedObject<object> decodedTag, Type objectClass, ElementInfo elementInfo, Stream stream)  ;
        DecodedObject<object> decodeOctetString(DecodedObject<object> decodedTag, Type objectClass, ElementInfo elementInfo, Stream stream)  ;
        DecodedObject<object> decodeBitString(DecodedObject<object> decodedTag, Type objectClass, ElementInfo elementInfo, Stream stream)  ;
        DecodedObject<object> decodeObjectIdentifier(DecodedObject<object> decodedTag, Type objectClass, ElementInfo elementInfo, Stream stream);
        DecodedObject<object> decodeString(DecodedObject<object> decodedTag, Type objectClass, ElementInfo elementInfo, Stream stream)  ;
        DecodedObject<object> decodeSequenceOf(DecodedObject<object> decodedTag, Type objectClass, ElementInfo elementInfo, Stream stream)  ;    
        DecodedObject<object> decodeEnum(DecodedObject<object> decodedTag,Type objectClass, ElementInfo elementInfo, Stream stream) ;
        DecodedObject<object> decodeEnumItem(DecodedObject<object> decodedTag, Type objectClass, Type enumClass, ElementInfo elementInfo, Stream stream)  ;
        DecodedObject<object> decodeBoxedType(DecodedObject<object> decodedTag, Type objectClass, ElementInfo elementInfo, Stream stream) ;
        DecodedObject<object> decodeElement(DecodedObject<object> decodedTag,Type objectClass, ElementInfo elementInfo, Stream stream) ;
        DecodedObject<object> decodePreparedElement(DecodedObject<object> decodedTag,Type objectClass, ElementInfo elementInfo, Stream stream) ;
        void invokeSetterMethodForField(PropertyInfo field, object obj, Object param, ElementInfo elementInfo) ;
        void invokeSelectMethodForField(PropertyInfo field, object obj, Object param, ElementInfo elementInfo);
    }
}
