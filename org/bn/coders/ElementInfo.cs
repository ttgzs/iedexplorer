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
using org.bn.attributes;
using org.bn.metadata;

namespace org.bn.coders
{	
	public class ElementInfo
	{
        private ASN1Element element;
        private ICustomAttributeProvider parentAnnotatedClass;

        public ICustomAttributeProvider ParentAnnotatedClass
        {
            get { return parentAnnotatedClass; }
            set { parentAnnotatedClass = value; }
        }

        public ASN1Element ASN1ElementInfo
        {
            get { return element; }
            set { element = value; }
        }
        private ICustomAttributeProvider annotatedClass;

        public ICustomAttributeProvider AnnotatedClass
        {
            get { return annotatedClass; }
            set { annotatedClass = value; }
        }

        private IASN1PreparedElementData preparedInfo;
        public IASN1PreparedElementData PreparedInfo
        {
            get { return preparedInfo; }
            set { 
                preparedInfo = value;
                if (preparedInfo != null)
                {
                    PreparedASN1ElementInfo = preparedInfo.ASN1ElementInfo;
                }       
            }
        }
        public bool hasPreparedInfo()
        {
            return this.preparedInfo != null;
        }


        private Object preparedInstance;
        public Object PreparedInstance
        {
            get { return preparedInstance; }
            set { preparedInstance = value; }
        }
        
        private ASN1ElementMetadata preparedElementMetadata;
        public ASN1ElementMetadata PreparedASN1ElementInfo
        {
            get { return preparedElementMetadata; }
            set { preparedElementMetadata = value; }
        }

        public bool hasPreparedASN1ElementInfo()
        {
            return this.preparedElementMetadata != null;
        }

        public PropertyInfo[] getProperties(Type objClass)
        {
            if (hasPreparedInfo())
            {
                return PreparedInfo.Properties;
            }
            else
                return objClass.GetProperties();
        }


        public ElementInfo()
        {
        }

        public bool isAttributePresent<T>() 
        {
            return CoderUtils.isAttributePresent<T>(annotatedClass);
        }

        public T getAttribute<T>()
        {
            return CoderUtils.getAttribute<T>(annotatedClass);
        }

        public bool isParentAttributePresent<T>()
        {
            return CoderUtils.isAttributePresent<T>(parentAnnotatedClass);
        }

        public T getParentAttribute<T>()
        {
            return CoderUtils.getAttribute<T>(parentAnnotatedClass);
        }

        private int maxAvailableLen = -1;

        public int MaxAvailableLen
        {
            get { return maxAvailableLen; }
            set { maxAvailableLen = value; }
        }
        

	}
}
