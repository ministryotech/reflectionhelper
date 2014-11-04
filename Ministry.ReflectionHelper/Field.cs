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
using Ministry.StrongTyped;

namespace Ministry.ReflectionHelper
{
    /// <summary>
    /// Functions to simplify access to field data using Reflection.
    /// </summary>
    public static class Field
    {
        #region | Exists |

        /// <summary>
        /// Confirms the existence of a field on an object.
        /// </summary>
        /// <typeparam name="T">The type of the object to set.</typeparam>
        /// <param name="value">The object to look for a field on.</param>
        /// <param name="fieldName">The name of the field to look for.</param>
        public static bool Exists<T>(T value, string fieldName)
        {
            CheckParameter.IsNotNull(value, "value");
            CheckParameter.IsNotNullOrEmpty(fieldName, "fieldName");

            var fi = GetInfo(value.GetType(), fieldName, true);
            return (fi != null);
        }

        /// <summary>
        /// Confirms the existence of a field on an object.
        /// </summary>
        /// <param name="staticType">The type to look for a field on.</param>
        /// <param name="fieldName">The name of the field to look for.</param>
        public static bool Exists(Type staticType, string fieldName)
        {
            CheckParameter.IsNotNull(staticType, "staticType");
            CheckParameter.IsNotNullOrEmpty(fieldName, "fieldName");

            var fi = GetInfo(staticType, fieldName, true);
            return (fi != null);
        }

        #endregion

        #region | Get |

        /// <summary>
        /// Gets the value of an object's field.
        /// </summary>
        /// <param name="value">The object to get the field of.</param>
        /// <param name="fieldName">The name of the field to get.</param>
        /// <returns>The value of the field, or null.</returns>
        /// <exception cref="System.ArgumentNullException">The value or fieldName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The fieldName specified is invalid.</exception>
        public static object Get(object value, string fieldName)
        {
            CheckParameter.IsNotNull(value, "value");
            CheckParameter.IsNotNullOrEmpty(fieldName, "fieldName");

            var fi = GetInfo(value.GetType(), fieldName);
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
        public static T Get<T>(object value, string fieldName)
        {
            CheckParameter.IsNotNull(value, "value");
            CheckParameter.IsNotNullOrEmpty(fieldName, "fieldName");

            var retVal = default(T);
            if (Get(value, fieldName) is T) retVal = (T)Get(value, fieldName);
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
        public static object Get(Type staticType, string fieldName)
        {
            CheckParameter.IsNotNull(staticType, "staticType");
            CheckParameter.IsNotNullOrEmpty(fieldName, "fieldName");

            var fi = GetInfo(staticType, fieldName);
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
        public static T Get<T>(Type staticType, string fieldName)
        {
            CheckParameter.IsNotNull(staticType, "staticType");
            CheckParameter.IsNotNullOrEmpty(fieldName, "fieldName");

            var retVal = default(T);
            if (Get(staticType, fieldName) is T) retVal = (T)Get(staticType, fieldName);
            return retVal;
        }

        #endregion

        #region | GetInfo |

        /// <summary>
        /// Searches recursively through an object tree for field information.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="fieldName">The name of the field to search for.</param>
        /// <returns>
        /// A FieldInfo object for analysing the field data.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">The type or fieldName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The fieldName specified is invalid.</exception>
        public static FieldInfo GetInfo(object value, string fieldName)
        {
            CheckParameter.IsNotNull(value, "value");
            CheckParameter.IsNotNullOrEmpty(fieldName, "fieldName");

            return GetInfo(value.GetType(), fieldName);
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
        private static FieldInfo GetInfo(Type type, string fieldName, bool suppressExceptions = false)
        {
            CheckParameter.IsNotNull(type, "type");
            CheckParameter.IsNotNullOrEmpty(fieldName, "fieldName");

            var fi = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);
            if (fi != null) return fi;

            if (type.BaseType != null)
            {
                fi = GetInfo(type.BaseType, fieldName, suppressExceptions);
            }
            else if (!suppressExceptions)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The field name specified ({0}) does not exist on the object specified.", fieldName), "fieldName");
            }

            return fi;
        }

        #endregion

        #region | Set |

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
        public static void Set<T, TField>(T value, string fieldName, TField fieldValue)
        {
            CheckParameter.IsNotNull(value, "value");
            CheckParameter.IsNotNullOrEmpty(fieldName, "fieldName");

            var fi = GetInfo(value.GetType(), fieldName);
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
        public static void Set<TField>(Type staticType, string fieldName, TField fieldValue)
        {
            CheckParameter.IsNotNull(staticType, "staticType");
            CheckParameter.IsNotNullOrEmpty(fieldName, "fieldName");

            var fi = GetInfo(staticType, fieldName);
            fi.SetValue(null, fieldValue);
        }

        #endregion
    }
}
