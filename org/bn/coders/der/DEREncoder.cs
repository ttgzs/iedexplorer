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
using System.Collections.Generic;
using org.bn.utils;
using org.bn.attributes;
using org.bn.coders.ber;

namespace org.bn.coders.der
{
    class DEREncoder: BEREncoder
    {
        public override int encodeSequence(Object obj, System.IO.Stream stream, ElementInfo elementInfo)
        {
            if(!CoderUtils.isSequenceSet(elementInfo))
                return base.encodeSequence(obj, stream, elementInfo);
            else {
                int resultSize = 0;
                PropertyInfo[] fields = null;
                if (elementInfo.hasPreparedInfo())
                {
                    fields = elementInfo.getProperties(obj.GetType());
                }
                else
                {
                    SortedList<int, PropertyInfo> fieldOrder = CoderUtils.getSetOrder(obj.GetType());
                    //TO DO Performance optimization need (unnecessary copy)
                    fields = new PropertyInfo[fieldOrder.Count];
                    fieldOrder.Values.CopyTo(fields, 0);
                }

                for (int i = 0; i < fields.Length; i++)
                {
                    PropertyInfo field = fields[fields.Length - 1 - i];
                    resultSize += encodeSequenceField(obj, fields.Length - 1 - i, field, stream, elementInfo);
                }

                resultSize += encodeHeader(
                    BERCoderUtils.getTagValueForElement(
                        elementInfo,
                        TagClasses.Universal,
                        ElementType.Constructed,
                        UniversalTags.Set)
                    , resultSize, stream);
                return resultSize;
            }
        }

    }
}
