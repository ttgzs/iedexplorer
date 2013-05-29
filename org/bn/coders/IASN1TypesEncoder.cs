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
    public interface IASN1TypesEncoder {
        int encodeClassType(object obj, Stream stream, ElementInfo elementInfo) ;
        int encodeSequence(object obj, Stream stream, ElementInfo elementInfo) ;
        int encodeChoice(object obj, Stream stream, ElementInfo elementInfo)  ;
        int encodeEnum(object obj, Stream stream, ElementInfo elementInfo) ;
        int encodeEnumItem(Object enumConstant, Type enumClass, Stream stream, ElementInfo elementInfo)  ;
        int encodeBoolean(object obj, Stream stream, ElementInfo elementInfo) ;
        int encodeAny(object obj, Stream stream, ElementInfo elementInfo)  ;
        int encodeNull(object obj, Stream stream, ElementInfo elementInfo)  ;
        int encodeInteger(object obj, Stream steam, ElementInfo elementInfo)  ;
        int encodeReal(object obj, Stream steam, ElementInfo elementInfo)  ;
        int encodeOctetString(object obj, Stream steam, ElementInfo elementInfo)  ;
        int encodeBitString(object obj, Stream steam, ElementInfo elementInfo)  ;
        int encodeObjectIdentifier(object obj, Stream steam, ElementInfo elementInfo);
        int encodeString(object obj, Stream steam, ElementInfo elementInfo)  ;
        int encodeSequenceOf(object obj, Stream steam, ElementInfo elementInfo)  ;   
        int encodeElement(object obj, Stream stream, ElementInfo elementInfo) ;
        int encodeBoxedType(object obj, Stream stream, ElementInfo elementInfo) ;
        int encodePreparedElement(object obj, Stream stream, ElementInfo elementInfo) ;
        object invokeGetterMethodForField(PropertyInfo field, object obj, ElementInfo elementInfo) ;
        bool invokeSelectedMethodForField(PropertyInfo field, object obj, ElementInfo elementInfo);
    }
}