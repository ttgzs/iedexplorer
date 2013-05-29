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
using org.bn.attributes;
using org.bn.coders;

namespace org.bn.coders.per
{
	
	public class PERCoderUtils
	{
		public static int getMaxBitLength(long val)
		{
			int bitCnt = 0;
			while (val != 0)
			{
                if (val >= 0)
                    val = val >> 1;
                else
                    val = (val >> 1) + (2 << ~1);
				bitCnt++;
			}
			return bitCnt;
		}

        public static bool is7BitEncodedString(ElementInfo info)
        {
            bool is7Bit = false;
            int stringType = CoderUtils.getStringTagForElement(info);
            is7Bit = (
                stringType == org.bn.coders.UniversalTags.PrintableString
                || stringType == org.bn.coders.UniversalTags.VisibleString
            );
            return is7Bit;
        }

	}
}