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
using org.bn.metadata;
using org.bn.metadata.constraints;

namespace org.bn.coders
{

    public interface IASN1PreparedElementData {
        ASN1Metadata TypeMetadata
        {
            get;
        }
        
        IASN1ConstraintMetadata Constraint
        {
            get;
        }

        bool hasConstraint();


        PropertyInfo[] Properties
        {
            get;
        }

        PropertyInfo getProperty(int index);
        ASN1PreparedElementData getPropertyMetadata(int index);

        PropertyInfo ValueProperty
        {
            get;
        }

        ASN1PreparedElementData ValueMetadata
        {
            get;
        }
        
        ASN1ElementMetadata ASN1ElementInfo
        {
            get;
        }

        bool hasASN1ElementInfo();
        
        object invokeDoSelectMethod(object obj, object param) ;
        object invokeIsSelectedMethod(object obj, object param) ;
        //object invokeIsPresentMethod(object obj, object param);
        MethodInfo IsPresentMethod
        {
            get;
        }
    }
}

