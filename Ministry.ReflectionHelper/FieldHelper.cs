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
    /// Functions to simplify access to field data using Reflection.
    /// </summary>
    /// <remarks>
    /// Keith Jackson
    /// 04/02/2011
    /// </remarks>
    public class FieldHelper
    {

        #region | Constants |

        private const string PARAM_Value = "value";
        private const string PARAM_StaticType = "staticType";
        private const string PARAM_Type = "type";

        private const string PARAM_FieldName = "fieldName";

        private const string ERR_InvalidField = "The field name specified ({0}) does not exist on the object specified.";

        #endregion

        #region | Construction |

        internal FieldHelper() { }

        #endregion

        #region | Public Methods |

        #region | FieldExists |

        /// <summary>
        /// Confirms the existence of a field on an object.
        /// </summary>
        /// <typeparam name="T">The type of the object to set.</typeparam>
        /// <param name="value">The object to look for a field on.</param>
        /// <param name="fieldName">The name of the field to look for.</param>
        public bool FieldExists<T>(T value, string fieldName)
        {
            FieldInfo fi = GetFieldInfo(value.GetType(), fieldName, true);
            return (fi != null);
        }

        /// <summary>
        /// Confirms the existence of a field on an object.
        /// </summary>
        /// <param name="staticType">The type to look for a field on.</param>
        /// <param name="fieldName">The name of the field to look for.</param>
        public bool FieldExists(Type staticType, string fieldName)
        {
            FieldInfo fi = GetFieldInfo(staticType, fieldName, true);
            return (fi != null);
        }

        #endregion

        #region | GetField |

        /// <summary>
        /// Gets the value of an object's field.
        /// </summary>
        /// <param name="value">The object to get the field of.</param>
        /// <param name="fieldName">The name of the field to get.</param>
        /// <returns>The value of the field, or null.</returns>
        /// <exception cref="System.ArgumentNullException">The value or fieldName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The fieldName specified is invalid.</exception>
        public object GetField(object value, string fieldName)
        {
            if (value == null) throw new ArgumentNullException(PARAM_Value);
            FieldInfo fi = GetFieldInfo(value.GetType(), fieldName);
            return fi.GetValue(value);
        }

        /// <summary>
        /// Gets the value of an object's field.
        /// </summary>
        /// <typeparam name="T">The type of the field to return.</typeparam>
        /// <param name="value">The object to get the field of.</param>
        /// <param name="fieldName">The name of the field to get.</param>
        /// <returns>The value of the field, or null.</returns>
        /// <exception cref="System.ArgumentNullException">The value or fieldName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The fieldName specified is invalid.</exception>
        public T GetField<T>(object value, string fieldName)
        {
            T retVal = default(T);
            if (GetField(value, fieldName) is T) retVal = (T)GetField(value, fieldName);
            return retVal;
        }

        /// <summary>
        /// Gets the value of an object's field.
        /// </summary>
        /// <param name="staticType">The static type to get the field of.</param>
        /// <param name="fieldName">The name of the field to get.</param>
        /// <returns>The value of the field, or null.</returns>
        /// <exception cref="System.ArgumentNullException">The staticType or fieldName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The fieldName specified is invalid.</exception>
        public object GetField(Type staticType, string fieldName)
        {
            if (staticType == null) throw new ArgumentNullException(PARAM_StaticType);
            FieldInfo fi = GetFieldInfo(staticType, fieldName);
            return fi.GetValue(null);
        }

        /// <summary>
        /// Gets the value of an object's field.
        /// </summary>
        /// <typeparam name="T">The type of the field to return.</typeparam>
        /// <param name="staticType">The static type to get the field of.</param>
        /// <param name="fieldName">The name of the field to get.</param>
        /// <returns>The value of the field, or null.</returns>
        /// <exception cref="System.ArgumentNullException">The staticType or fieldName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The fieldName specified is invalid.</exception>
        public T GetField<T>(Type staticType, string fieldName)
        {
            T retVal = default(T);
            if (GetField(staticType, fieldName) is T) retVal = (T)GetField(staticType, fieldName);
            return retVal;
        }

        #endregion

        #region | GetFieldInfo |

        /// <summary>
        /// Searches recursively through an object tree for field information.
        /// </summary>
        /// <param name="type">The type of the object to get the field from.</param>
        /// <param name="fieldName">The name of the field to search for.</param>
        /// <returns>A FieldInfo object for analysing the field data.</returns>
        /// <exception cref="System.ArgumentNullException">The type or fieldName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The fieldName specified is invalid.</exception>
        public FieldInfo GetFieldInfo(object value, string fieldName)
        {
            if (value == null) throw new ArgumentNullException(PARAM_Value);
            return GetFieldInfo(value.GetType(), fieldName, false);
        }

        /// <summary>
        /// Searches recursively through an object tree for field information.
        /// </summary>
        /// <param name="type">The type of the object to get the field from.</param>
        /// <param name="fieldName">The name of the field to search for.</param>
        /// <returns>A FieldInfo object for analysing the field data.</returns>
        /// <exception cref="System.ArgumentNullException">The type or fieldName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The fieldName specified is invalid.</exception>
        public FieldInfo GetFieldInfo(Type type, string fieldName)
        {
            return GetFieldInfo(type, fieldName, false);
        }

        /// <summary>
        /// Searches recursively through an object tree for field information.
        /// </summary>
        /// <param name="type">The type of the object to get the field from.</param>
        /// <param name="fieldName">The name of the field to search for.</param>
        /// <param name="suppressExceptions">Specifies that exceptions for invalid fields should be suppressed.</param>
        /// <returns>A FieldInfo object for analysing the field data.</returns>
        /// <exception cref="System.ArgumentNullException">The type or fieldName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The fieldName specified is invalid.</exception>
        private FieldInfo GetFieldInfo(Type type, string fieldName, bool suppressExceptions)
        {
            if (type == null) throw new ArgumentNullException(PARAM_Type);
            if (fieldName == null) throw new ArgumentNullException(PARAM_FieldName);
            FieldInfo fi = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);
            if (fi == null)
            {
                if (type.BaseType != null)
                {
                    fi = GetFieldInfo(type.BaseType, fieldName, suppressExceptions);
                }
                else if (!suppressExceptions)
                {
                    throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, ERR_InvalidField, fieldName));
                }
            }

            return fi;
        }

        #endregion

        #region | SetField |

        /// <summary>
        /// Sets the value of an object's field.
        /// </summary>
        /// <typeparam name="T">The type of the object to set.</typeparam>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="value">The object to set the field of.</param>
        /// <param name="fieldName">The name of the field to set.</param>
        /// <param name="fieldValue">The value to set on the field.</param>
        /// <exception cref="System.ArgumentNullException">The value or fieldName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The fieldName specified is invalid.</exception>
        public void SetField<T, TField>(T value, string fieldName, TField fieldValue)
        {
            if (value == null) throw new ArgumentNullException(PARAM_Value);
            FieldInfo fi = GetFieldInfo(value.GetType(), fieldName);
            fi.SetValue(value, fieldValue);
        }

        /// <summary>
        /// Sets the value of an object's field.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="staticType">The static type to set the field of.</param>
        /// <param name="fieldName">The name of the field to set.</param>
        /// <param name="fieldValue">The value to set on the field.</param>
        /// <exception cref="System.ArgumentNullException">The staticType or fieldName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The fieldName specified is invalid.</exception>
        public void SetField<TField>(Type staticType, string fieldName, TField fieldValue)
        {
            if (staticType == null) throw new ArgumentNullException(PARAM_StaticType);
            FieldInfo fi = GetFieldInfo(staticType, fieldName);
            fi.SetValue(null, fieldValue);
        }

        #endregion

        #endregion

    }
}
