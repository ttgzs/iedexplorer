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
using org.bn.coders;

namespace org.bn.metadata
{

    public class ASN1BoxedTypeMetadata  : ASN1FieldMetadata {
        private PropertyInfo valueField;
        private ASN1PreparedElementData valueFieldMeta;
        
        public ASN1BoxedTypeMetadata(String name) : base(name)
        {            
        }

        public ASN1BoxedTypeMetadata(Type objClass, ASN1BoxedType annotation)
            : this(annotation.Name)
        {            
            setupValueField(objClass);
        }               
        
        public void setupValueField(Type objClass) {
            try
            {
                valueField = objClass.GetProperty("Value"); //getDeclaredField("value");
                valueFieldMeta = new ASN1PreparedElementData(objClass, valueField);
            }
            catch (Exception) { }
        }

        public override int encode(IASN1TypesEncoder encoder, object obj, Stream stream, ElementInfo elementInfo)
        {
            Object result = null;
            ASN1ElementMetadata saveInfo = elementInfo.PreparedASN1ElementInfo;        
            elementInfo.PreparedInfo = (valueFieldMeta);        
            if(!CoderUtils.isNullField(valueField,elementInfo)) {
                result = encoder.invokeGetterMethodForField(valueField, obj, elementInfo);
            }        
            if(saveInfo!=null) {
                if (!saveInfo.HasTag 
                    && elementInfo.hasPreparedASN1ElementInfo()
                    && elementInfo.PreparedASN1ElementInfo.HasTag)
                {
                    ASN1ElementMetadata elData = new ASN1ElementMetadata(
                        saveInfo.Name,
                        saveInfo.IsOptional,
                        elementInfo.PreparedASN1ElementInfo.HasTag,
                        elementInfo.PreparedASN1ElementInfo.IsImplicitTag,
                        elementInfo.PreparedASN1ElementInfo.TagClass,
                        elementInfo.PreparedASN1ElementInfo.Tag,
                        saveInfo.HasDefaultValue
                    );
                    elementInfo.PreparedASN1ElementInfo = elData;
                }
                else
                    elementInfo.PreparedASN1ElementInfo = (saveInfo);
            }
            return valueFieldMeta.TypeMetadata.encode(encoder,result, stream, elementInfo);
        }

        public override DecodedObject<object> decode(IASN1TypesDecoder decoder, DecodedObject<object> decodedTag, Type objectClass, ElementInfo elementInfo, Stream stream)
        {
             IASN1PreparedElementData saveInfo = elementInfo.PreparedInfo;            
             IASN1PreparedElement instance =  (IASN1PreparedElement)elementInfo.PreparedInstance;
             ASN1ElementMetadata saveElemInfo = elementInfo.PreparedASN1ElementInfo;        
             elementInfo.PreparedInfo = (valueFieldMeta);
            
            if(saveElemInfo!=null) {
                if (!saveElemInfo.HasTag
                        && elementInfo.hasPreparedASN1ElementInfo()
                        && elementInfo.PreparedASN1ElementInfo.HasTag)
                {
                    ASN1ElementMetadata elData = new ASN1ElementMetadata(
                        saveElemInfo.Name,
                        saveElemInfo.IsOptional,
                        elementInfo.PreparedASN1ElementInfo.HasTag,
                        elementInfo.PreparedASN1ElementInfo.IsImplicitTag,
                        elementInfo.PreparedASN1ElementInfo.TagClass,
                        elementInfo.PreparedASN1ElementInfo.Tag,
                        saveElemInfo.HasDefaultValue
                    );
                    elementInfo.PreparedASN1ElementInfo = elData;
                }
                else
                    elementInfo.PreparedASN1ElementInfo = (saveElemInfo);
            }
            DecodedObject<object> decodedResult = 
                valueFieldMeta.TypeMetadata.decode(decoder,decodedTag,valueField.PropertyType,elementInfo,stream);
            if(decodedResult!=null) {
                if (!CoderUtils.isNullField(valueField, elementInfo))
                {
                    decoder.invokeSetterMethodForField(valueField, instance, decodedResult.Value, elementInfo);
                }
            }
            elementInfo.PreparedInfo = (saveInfo);
            elementInfo.PreparedInstance = (instance);
            elementInfo.PreparedASN1ElementInfo = (saveElemInfo);
            
            if(decodedResult!=null)
                return new DecodedObject<object>(instance,decodedResult.Size);
            else
                return decodedResult;
        }    
        
    }
}
