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
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using org.bn.attributes;
using org.bn.coders;

namespace org.bn.metadata
{
    public class ASN1SequenceOfMetadata : ASN1FieldMetadata
    {
        private bool isSetOf = false;
        private Type itemClass;
        private ASN1PreparedElementData itemClassMetadata;

        public ASN1SequenceOfMetadata(String name, bool isSetOf, Type itemClass, ICustomAttributeProvider seqFieldAnnotatedElem)
            : base(name)
        {
            this.isSetOf = isSetOf;
            this.itemClass = itemClass;
            Type paramType = itemClass.GetGenericArguments()[0];
            this.itemClassMetadata = new ASN1PreparedElementData(paramType);
            if (this.itemClassMetadata.TypeMetadata != null)
                this.itemClassMetadata.TypeMetadata.setParentAnnotated(seqFieldAnnotatedElem);
        }

        public ASN1SequenceOfMetadata(ASN1SequenceOf annotation, Type itemClass, ICustomAttributeProvider seqFieldAnnotatedElem)
            : this(annotation.Name, annotation.IsSetOf, itemClass, seqFieldAnnotatedElem)
        {
        }               
        
        public bool IsSetOf {
            get { return this.isSetOf; }
        }
        
        public IASN1PreparedElementData getItemClassMetadata() {
            return itemClassMetadata;
        }

        public override int encode(IASN1TypesEncoder encoder, object obj, Stream stream, ElementInfo elementInfo) 
        {
            return encoder.encodeSequenceOf(obj, stream, elementInfo);
        }

        public override DecodedObject<object> decode(IASN1TypesDecoder decoder, DecodedObject<object> decodedTag, Type objectClass, ElementInfo elementInfo, Stream stream)
        {
            return decoder.decodeSequenceOf(decodedTag,objectClass,elementInfo,stream);
        }
    }
}
