// Copyright (c) 2014 Minotech Ltd.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
// (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, 
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do 
// so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Reflection;
using System.Globalization;

namespace Ministry.Reflection
{

    /// <summary>
    /// Functions to simplify access to items using Reflection.
    /// </summary>
    /// <remarks>
    /// Keith Jackson
    /// 04/02/2011
    /// </remarks>
    public static class Helper
    {

        #region | Construction |

        static Helper()
        {
            Fields = new FieldHelper();
            Methods = new MethodHelper();
            Properties = new PropertyHelper();
        }

        #endregion

        #region | Properties |

        /// <summary>
        /// Functions to work with Fields.
        /// </summary>
        public static FieldHelper Fields { get; set; }

        /// <summary>
        /// Functions to work with Methods.
        /// </summary>
        public static MethodHelper Methods { get; set; }

        /// <summary>
        /// Functions to work with Properties.
        /// </summary>
        public static PropertyHelper Properties { get; set; }

        #endregion

    }
}
