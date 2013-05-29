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
using System.IO;
using org.bn.attributes;
using org.bn.coders;

namespace org.bn.metadata
{
    public class ASN1ElementMetadata : ASN1FieldMetadata
    {
        private bool isOptional = true;

        public bool IsOptional
        {
            get { return isOptional; }
        }


        private bool hasTag = false;

        public bool HasTag
        {
            get { return hasTag; }
        }
        private bool isImplicitTag = false;

        public bool IsImplicitTag
        {
            get { return isImplicitTag; }
        }
        private int tagClass = TagClasses.ContextSpecific;

        public int TagClass
        {
            get { return tagClass; }
        }
        private int tag = 0;

        public int Tag
        {
            get { return tag; }
        }
        private bool hasDefaultValue = false;

        public bool HasDefaultValue
        {
            get { return hasDefaultValue; }
        }

        public ASN1ElementMetadata(ASN1Element annotation) : 
            this(
                annotation.Name,
                annotation.IsOptional,
                annotation.HasTag,
                annotation.IsImplicitTag,
                annotation.TagClass,
                annotation.Tag,
                annotation.HasDefaultValue
            )
        {
        }

        public ASN1ElementMetadata(String name,
                                   bool isOptional,
                                   bool hasTag,
                                   bool isImplicitTag,
                                   int tagClass,
                                   int tag,
                                   bool hasDefaultValue)
            : base(name)
        {
            this.isOptional = isOptional;
            this.hasTag = hasTag;
            this.isImplicitTag = isImplicitTag;
            this.tagClass = tagClass;
            this.tag = tag;
            this.hasDefaultValue = hasDefaultValue;
        }

        public override int encode(IASN1TypesEncoder encoder, object obj, Stream stream, ElementInfo elementInfo)
        {
            return encoder.encodePreparedElement(obj, stream, elementInfo);
        }

        public override DecodedObject<object> decode(IASN1TypesDecoder decoder, DecodedObject<object> decodedTag, Type objectClass, ElementInfo elementInfo, Stream stream)
        {
            elementInfo.PreparedInstance = null;
            return decoder.decodeElement(decodedTag, objectClass, elementInfo, stream);
        }
    }
    
}
