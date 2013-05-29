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
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using org.bn.attributes;
using org.bn.attributes.constraints;
using org.bn.metadata;
using org.bn.metadata.constraints;
using org.bn.types;

namespace org.bn.coders
{
    public class ASN1PreparedElementData: IASN1PreparedElementData
    {
        private ASN1Metadata typeMeta;
        public ASN1Metadata TypeMetadata
        {
            get { return typeMeta; }
        }

        private IASN1ConstraintMetadata constraint;
        public IASN1ConstraintMetadata Constraint
        {
            get { return this.constraint; }
        }

        public bool hasConstraint()
        {
            return this.constraint != null;
        }

        private PropertyInfo[] properties;
        public PropertyInfo[] Properties
        {
            get { return properties; }
        }

        public PropertyInfo getProperty(int index)
        {
            return properties[index];
        }

        private ASN1PreparedElementData[] propertiesMetadata;

        public ASN1PreparedElementData getPropertyMetadata(int index)
        {
            return propertiesMetadata[index];
        }

        private PropertyInfo valueProperty;
        public PropertyInfo ValueProperty
        {
            get { return valueProperty; }
        }

        private ASN1PreparedElementData valueMetadata;
        public ASN1PreparedElementData ValueMetadata
        {
            get { return valueMetadata; }
        }

        ASN1ElementMetadata asn1ElementInfo;
        public ASN1ElementMetadata ASN1ElementInfo
        {
            get { return asn1ElementInfo; }
        }

        public bool hasASN1ElementInfo()
        {
            return asn1ElementInfo!=null;
        }

        private MethodInfo doSelectMethod;
        public object invokeDoSelectMethod(object obj, object param)
        {
            return doSelectMethod.Invoke(obj, new object[] { param } );
        }

        private MethodInfo isSelectedMethod;
        public object invokeIsSelectedMethod(object obj, object param)
        {
            return isSelectedMethod.Invoke(obj, null );
        }

        private MethodInfo isPresentMethod;
        public MethodInfo IsPresentMethod
        {
            get { return isPresentMethod; }
        }

        public ASN1PreparedElementData(Type objectClass)
        {
            setupMetadata(objectClass, objectClass);
            setupConstructed(objectClass);
        }

        public ASN1PreparedElementData(Type parentClass, PropertyInfo field)
        {
            setupMetadata(field, field.PropertyType);
            setupAccessors(parentClass, field);
        }

        /*public ASN1PreparedElementData(Type parentClass, String propertyName)
        {
            try
            {
                PropertyInfo field = parentClass.GetProperty(propertyName);
                setupMetadata(field, field.PropertyType);
                setupAccessors(parentClass, field);
            }
            catch (Exception ex)
            {
                ex = null;
            }
        }*/


        private void setupMetadata(ICustomAttributeProvider annotated, Type objectClass) {
            if (CoderUtils.isAttributePresent<ASN1_MMSDataArray>(annotated)) // Pavel
            {
                if (CoderUtils.isAttributePresent<ASN1SequenceOf>(annotated) && ASN1_MMSDataArray.Depth < 10)
                {
                    ASN1_MMSDataArray.Depth++;
                    typeMeta = new ASN1SequenceOfMetadata( CoderUtils.getAttribute<ASN1SequenceOf> (annotated),
                        objectClass,
                        annotated
                    ) ;
                }
       
            }
            else
                if (CoderUtils.isAttributePresent<ASN1_MMSDataStructure>(annotated)) // Pavel
            {
                if (CoderUtils.isAttributePresent<ASN1SequenceOf>(annotated) && ASN1_MMSDataStructure.Depth < 10)
                {
                    ASN1_MMSDataStructure.Depth++;
                    typeMeta = new ASN1SequenceOfMetadata( CoderUtils.getAttribute<ASN1SequenceOf> (annotated),
                        objectClass,
                        annotated
                    ) ;
                }
       
            }
            else
            if( CoderUtils.isAttributePresent<ASN1SequenceOf>(annotated) ) {
                typeMeta = new ASN1SequenceOfMetadata( CoderUtils.getAttribute<ASN1SequenceOf> (annotated),
                    objectClass,
                    annotated
                ) ;
            }        
            else    
            if( CoderUtils.isAttributePresent<ASN1Sequence>(annotated) ) {
                typeMeta = new ASN1SequenceMetadata( CoderUtils.getAttribute<ASN1Sequence> (annotated) ) ;
            }
            else
            if( CoderUtils.isAttributePresent<ASN1Choice>(annotated) ) {
                typeMeta = new ASN1ChoiceMetadata( CoderUtils.getAttribute<ASN1Choice> (annotated) ) ;
            }
            else
            if( CoderUtils.isAttributePresent<ASN1Enum>(annotated) ) {
                typeMeta = new ASN1EnumMetadata( CoderUtils.getAttribute<ASN1Enum> (annotated) ) ;
            }
            else
            if( CoderUtils.isAttributePresent<ASN1Boolean>(annotated) ) {
                typeMeta = new ASN1BooleanMetadata( CoderUtils.getAttribute<ASN1Boolean> (annotated) ) ;
            }
            else
            if( CoderUtils.isAttributePresent<ASN1Any>(annotated) ) {
                typeMeta = new ASN1AnyMetadata( CoderUtils.getAttribute<ASN1Any> (annotated) ) ;
            }
            else
            if( CoderUtils.isAttributePresent<ASN1Integer>(annotated) ) {
                typeMeta = new ASN1IntegerMetadata( CoderUtils.getAttribute<ASN1Integer> (annotated) ) ;
            }        
            else
            if( CoderUtils.isAttributePresent<ASN1Real>(annotated) ) {
                typeMeta = new ASN1RealMetadata( CoderUtils.getAttribute<ASN1Real> (annotated) ) ;
            }        
            else
            if( CoderUtils.isAttributePresent<ASN1OctetString>(annotated) ) {
                typeMeta = new ASN1OctetStringMetadata( CoderUtils.getAttribute<ASN1OctetString> (annotated) ) ;
            }
            else
            if (CoderUtils.isAttributePresent<ASN1BitString>(annotated) || objectClass.Equals(typeof(BitString)))
            {
                typeMeta = new ASN1BitStringMetadata ( CoderUtils.getAttribute<ASN1BitString> (annotated) ) ;
            }
            else
            if (CoderUtils.isAttributePresent<ASN1ObjectIdentifier>(annotated) || objectClass.Equals(typeof(ObjectIdentifier)))
            {
                typeMeta = new ASN1ObjectIdentifierMetadata(CoderUtils.getAttribute<ASN1ObjectIdentifier>(annotated));
            }
            else
            if( CoderUtils.isAttributePresent<ASN1String>(annotated) ) {
                typeMeta = new ASN1StringMetadata ( CoderUtils.getAttribute<ASN1String> (annotated) ) ;
            }
            else
            if( CoderUtils.isAttributePresent<ASN1Null>(annotated) ) {
                typeMeta = new ASN1NullMetadata ( CoderUtils.getAttribute<ASN1Null> (annotated) ) ;
            }
            else
            if( CoderUtils.isAttributePresent<ASN1BoxedType>(annotated) ) {
                typeMeta = new ASN1BoxedTypeMetadata ( objectClass, CoderUtils.getAttribute<ASN1BoxedType> (annotated) ) ;
            }        
            else
            if( CoderUtils.isAttributePresent<ASN1Element>(annotated) ) {
                typeMeta = new ASN1ElementMetadata ( CoderUtils.getAttribute<ASN1Element> (annotated) ) ;
            }
            else
            if(objectClass.Equals(typeof(String))) {
                typeMeta = new ASN1StringMetadata( ) ;
            }
            else
            if(objectClass.Equals(typeof(int))) {
                typeMeta = new ASN1IntegerMetadata( ) ;
            }
            else
            if(objectClass.Equals(typeof(long))) {
                typeMeta = new ASN1IntegerMetadata( ) ;
            }
            else
            if(objectClass.Equals(typeof(double))) {
                typeMeta = new ASN1RealMetadata( ) ;
            }
            else        
            if(objectClass.Equals(typeof(bool))) {
                typeMeta = new ASN1BooleanMetadata( ) ;
            }        
            else
            if(objectClass.Equals(typeof(byte[]))) {
                typeMeta = new ASN1OctetStringMetadata( ) ;
            }
            
            if( CoderUtils.isAttributePresent<ASN1Element>(annotated) ) {
                asn1ElementInfo = new ASN1ElementMetadata(CoderUtils.getAttribute<ASN1Element>(annotated));
            }

            //ASN1CyclicDefinition.Depth = 0; // Pavel
            setupConstraint(annotated);
        }

        private void setupConstructed(Type objectClass) 
        {
            int count = 0;
            PropertyInfo[] srcFields = null;
            if(typeMeta !=null && typeMeta is ASN1SequenceMetadata && ((ASN1SequenceMetadata)typeMeta).IsSet) {
                SortedList<int, PropertyInfo> fieldOrder = CoderUtils.getSetOrder(objectClass);
                srcFields = new PropertyInfo[fieldOrder.Count];
                fieldOrder.Values.CopyTo(srcFields, 0);
                count = srcFields.Length;
            }        
            else {
                srcFields = objectClass.GetProperties(); //objectClass.getDeclaredFields();
                foreach(PropertyInfo field in srcFields) {
                    if (!field.PropertyType.Equals(typeof(IASN1PreparedElementData)))
                    {
                        count++;
                    }
                }
            }

            properties = new PropertyInfo[count];
            propertiesMetadata = new ASN1PreparedElementData[count];
            int idx=0;
            foreach(PropertyInfo field in srcFields) {
                if (!field.PropertyType.Equals(typeof(IASN1PreparedElementData)))
                {
                    properties[idx] = field;
                    propertiesMetadata[idx] = new ASN1PreparedElementData(objectClass, field);

                    if (properties[idx].Name.Equals("Value"))
                    {
                        setValueField(field, propertiesMetadata[idx]);
                    }
                    idx++;
                }            
            }
        }

        public void setValueField(PropertyInfo valueProperty, ASN1PreparedElementData valuePropertyMeta)
        {
            this.valueProperty = valueProperty;
            this.valueMetadata = valuePropertyMeta;
        }

        private void setupAccessors(Type objectClass, PropertyInfo field)
        {
            try
            {
                this.doSelectMethod = CoderUtils.findDoSelectMethodForField(field, objectClass);
            }
            catch (Exception) {}

            try
            {
                isSelectedMethod = CoderUtils.findIsSelectedMethodForField (field, objectClass);
            }
            catch (Exception) {}

            try
            {
                isPresentMethod = CoderUtils.findIsPresentMethodForField (field, objectClass);
            }
            catch (Exception) {}

        }


        private void setupConstraint(ICustomAttributeProvider annotated)
        {
            if( CoderUtils.isAttributePresent<ASN1SizeConstraint>(annotated) ) {
                constraint = new ASN1SizeConstraintMetadata( CoderUtils.getAttribute<ASN1SizeConstraint> (annotated) ) ;
            }
            else
            if (CoderUtils.isAttributePresent<ASN1ValueRangeConstraint>(annotated))
            {
                constraint = new ASN1ValueRangeConstraintMetadata(CoderUtils.getAttribute<ASN1ValueRangeConstraint>(annotated));
            }

        }



    }
}
