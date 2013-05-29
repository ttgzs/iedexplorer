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
    public class ASN1SequenceMetadata: ASN1TypeMetadata
    {
        private bool isSet;

        public ASN1SequenceMetadata(ASN1Sequence annotation)
            : this(annotation.Name, annotation.IsSet)
        {
        }

        public ASN1SequenceMetadata(String  name,
                                    bool isSet): base(name)
        {            
            this.isSet = isSet;
        }

        public bool IsSet
        {
            get { return isSet; }
        }

        public override int encode(IASN1TypesEncoder encoder, object obj, Stream stream, ElementInfo elementInfo)
        {
            return encoder.encodeSequence(obj, stream, elementInfo);
        }

        public override DecodedObject<object> decode(IASN1TypesDecoder decoder, DecodedObject<object> decodedTag, Type objectClass, ElementInfo elementInfo, Stream stream)
        {
            return decoder.decodeSequence(decodedTag,objectClass,elementInfo,stream);
        }    
        
    }
}
