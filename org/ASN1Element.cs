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
using org.bn.coders;

namespace org.bn.attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ASN1Element : Attribute
    {
        private string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        bool isOptional = true;

        public bool IsOptional
        {
            get { return isOptional; }
            set { isOptional = value; }
        }
        bool hasTag = false;

        public bool HasTag
        {
            get { return hasTag; }
            set { hasTag = value; }
        }

        bool isImplicitTag = true;

        public bool IsImplicitTag
        {
            get { return isImplicitTag; }
            set { isImplicitTag = value; }
        }
        
        int tagClass = TagClasses.ContextSpecific;

        public int TagClass
        {
            get { return tagClass; }
            set { tagClass = value; }
        }
        
        int tag = 0;

        public int Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        bool hasDefaultValue = false;

        public bool HasDefaultValue
        {
            get { return hasDefaultValue; }
            set { hasDefaultValue = value; }
        }


    }
}
