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
    /// Functions to simplify access to property data using Reflection.
    /// </summary>
    public static class Property
    {
        #region | Get |

        /// <summary>
        /// Gets the value of an object's property.
        /// </summary>
        /// <param name="value">The object to get the property of.</param>
        /// <param name="propertyName">The name of the property to get.</param>
        /// <returns>The value of the property, or null.</returns>
        /// <exception cref="System.ArgumentNullException">The value or propertyName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The propertyName specified is invalid.</exception>
        public static object Get(object value, string propertyName)
        {
            CheckParameter.IsNotNull(value, "value");
            CheckParameter.IsNotNullOrEmpty(propertyName, "propertyName");

            var pi = GetInfo(value.GetType(), propertyName, PropertyAccessRequired.Get);
            return pi.GetValue(value, null);
        }

        /// <summary>
        /// Gets the value of an object's property.
        /// </summary>
        /// <typeparam name="T">The type of the property to return.</typeparam>
        /// <param name="value">The object to get the property of.</param>
        /// <param name="propertyName">The name of the property to get.</param>
        /// <returns>The value of the property, or null.</returns>
        /// <exception cref="System.ArgumentNullException">The value or propertyName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The propertyName specified is invalid.</exception>
        public static T Get<T>(object value, string propertyName)
        {
            CheckParameter.IsNotNull(value, "value");
            CheckParameter.IsNotNullOrEmpty(propertyName, "propertyName");

            var retVal = default(T);
            if (Get(value, propertyName) is T) retVal = (T) Get(value, propertyName);
            return retVal;
        }

        /// <summary>
        /// Gets the value of an object's property.
        /// </summary>
        /// <param name="staticType">The static type to get the property of.</param>
        /// <param name="propertyName">The name of the property to get.</param>
        /// <returns>The value of the property, or null.</returns>
        /// <exception cref="System.ArgumentNullException">The staticType or propertyName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The propertyName specified is invalid.</exception>
        public static object Get(Type staticType, string propertyName)
        {
            CheckParameter.IsNotNull(staticType, "staticType");
            CheckParameter.IsNotNullOrEmpty(propertyName, "propertyName");

            var pi = GetInfo(staticType, propertyName, PropertyAccessRequired.Get);
            return pi.GetValue(null, null);
        }

        /// <summary>
        /// Gets the value of an object's property.
        /// </summary>
        /// <typeparam name="T">The type of the property to return.</typeparam>
        /// <param name="staticType">The static type to get the property of.</param>
        /// <param name="propertyName">The name of the property to get.</param>
        /// <returns>The value of the property, or null.</returns>
        /// <exception cref="System.ArgumentNullException">The staticType or propertyName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The propertyName specified is invalid.</exception>
        public static T Get<T>(Type staticType, string propertyName)
        {
            CheckParameter.IsNotNull(staticType, "staticType");
            CheckParameter.IsNotNullOrEmpty(propertyName, "propertyName");

            var retVal = default(T);
            if (Get(staticType, propertyName) is T) retVal = (T) Get(staticType, propertyName);
            return retVal;
        }

        #endregion

        #region | GetInfo |

        /// <summary>
        /// Searches recursively through an object tree for property information.
        /// </summary>
        /// <param name="value">The object to get the property from.</param>
        /// <param name="propertyName">The name of the property to search for.</param>
        /// <returns>A PropertyInfo object for analysing the property data.</returns>
        /// <exception cref="System.ArgumentNullException">The type or propertyName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The propertyName specified is invalid.</exception>
        public static PropertyInfo GetInfo(object value, string propertyName)
        {
            CheckParameter.IsNotNull(value, "value");
            CheckParameter.IsNotNullOrEmpty(propertyName, "propertyName");

            return GetInfo(value.GetType(), propertyName);
        }

        /// <summary>
        /// Searches recursively through an object tree for property information.
        /// </summary>
        /// <param name="type">The type of the object to get the property from.</param>
        /// <param name="propertyName">The name of the property to search for.</param>
        /// <returns>A PropertyInfo object for analysing the property data.</returns>
        /// <exception cref="System.ArgumentNullException">The type or propertyName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The propertyName specified is invalid.</exception>
        public static PropertyInfo GetInfo(Type type, string propertyName)
        {
            return GetInfo(type, propertyName, PropertyAccessRequired.Get, false);
        }

        /// <summary>
        /// Searches recursively through an object tree for property information.
        /// </summary>
        /// <param name="type">The type of the object to get the property from.</param>
        /// <param name="propertyName">The name of the property to search for.</param>
        /// <param name="access">The required property access.</param>
        /// <returns>A PropertyInfo object for analysing the property data.</returns>
        /// <exception cref="System.ArgumentNullException">The type or propertyName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The propertyName specified is invalid.</exception>
        public static PropertyInfo GetInfo(Type type, string propertyName, PropertyAccessRequired access)
        {
            return GetInfo(type, propertyName, access, false);
        }

        /// <summary>
        /// Searches recursively through an object tree for property information.
        /// </summary>
        /// <param name="type">The type of the object to get the property from.</param>
        /// <param name="propertyName">The name of the property to search for.</param>
        /// <param name="access">The required property access.</param>
        /// <param name="suppressExceptions">Specifies that exceptions for invalid fields should be suppressed.</param>
        /// <returns>A PropertyInfo object for analysing the property data.</returns>
        /// <exception cref="System.ArgumentNullException">The type or propertyName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The propertyName specified is invalid.</exception>
        private static PropertyInfo GetInfo(Type type, string propertyName, PropertyAccessRequired access,
            bool suppressExceptions)
        {
            CheckParameter.IsNotNull(type, "type");
            CheckParameter.IsNotNullOrEmpty(propertyName, "propertyName");

            var pi = type.GetProperty(propertyName,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly |
                BindingFlags.Static);
            var hasNoAccess = (pi != null && !HasRequiredAccess(pi, access));
            if (pi != null && !hasNoAccess) return pi;
            if (type.BaseType != null)
            {
                pi = GetInfo(type.BaseType, propertyName, access, suppressExceptions);
            }
            else if (!suppressExceptions)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture,
                    "The property name specified ({0}) does not exist on the object specified.", propertyName));
            }

            return pi;
        }

        #endregion

        #region | Exists |

        /// <summary>
        /// Confirms the existence of a property on an object.
        /// </summary>
        /// <typeparam name="T">The type of the object to set.</typeparam>
        /// <param name="value">The object to look for a property on.</param>
        /// <param name="propertyName">The name of the property to look for.</param>
        public static bool Exists<T>(T value, string propertyName)
        {
            CheckParameter.IsNotNull(value, "value");
            CheckParameter.IsNotNullOrEmpty(propertyName, "propertyName");

            var pi = GetInfo(value.GetType(), propertyName, PropertyAccessRequired.Get, true) ??
                     GetInfo(value.GetType(), propertyName, PropertyAccessRequired.Set, true);
            return (pi != null);
        }

        /// <summary>
        /// Confirms the existence of a property on an object.
        /// </summary>
        /// <param name="staticType">The type to look for a property on.</param>
        /// <param name="propertyName">The name of the property to look for.</param>
        public static bool Exists(Type staticType, string propertyName)
        {
            CheckParameter.IsNotNull(staticType, "staticType");
            CheckParameter.IsNotNullOrEmpty(propertyName, "propertyName");

            var fi = GetInfo(staticType, propertyName, PropertyAccessRequired.Get, true) ??
                     GetInfo(staticType, propertyName, PropertyAccessRequired.Set, true);
            return (fi != null);
        }

        #endregion

        #region | IsReadOnly |

        /// <summary>
        /// Determines if a property is read only.
        /// </summary>
        /// <param name="value">The object to get the property of.</param>
        /// <param name="propertyName">The name of the property to get.</param>
        /// <returns>A flag to indicate if a property is read only.</returns>
        public static bool IsReadOnly(object value, string propertyName)
        {
            CheckParameter.IsNotNull(value, "value");
            CheckParameter.IsNotNullOrEmpty(propertyName, "propertyName");

            var pi = GetInfo(value.GetType(), propertyName, PropertyAccessRequired.Get);
            return !HasRequiredAccess(pi, PropertyAccessRequired.Set);
        }

        /// <summary>
        /// Determines if a property is read only.
        /// </summary>
        /// <param name="staticType">The static type to get the property of.</param>
        /// <param name="propertyName">The name of the property to get.</param>
        /// <returns>A flag to indicate if a property is read only.</returns>
        public static bool IsReadOnly(Type staticType, string propertyName)
        {
            CheckParameter.IsNotNull(staticType, "staticType");
            CheckParameter.IsNotNullOrEmpty(propertyName, "propertyName");

            var pi = GetInfo(staticType, propertyName, PropertyAccessRequired.Get);
            return !HasRequiredAccess(pi, PropertyAccessRequired.Set);
        }

        #endregion

        #region | Set |

        /// <summary>
        /// Sets the value of an object's property.
        /// </summary>
        /// <param name="value">The object to set the property of.</param>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <param name="propertyValue">The value to set on the property.</param>
        /// <exception cref="System.ArgumentNullException">The value or propertyName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The propertyName specified is invalid and either does not exist or is read only.</exception>
        public static void Set(object value, string propertyName, object propertyValue)
        {
            CheckParameter.IsNotNull(value, "value");
            CheckParameter.IsNotNullOrEmpty(propertyName, "propertyName");

            if (IsReadOnly(value, propertyName))
                throw new InvalidOperationException(
                    "The property name specified ({0}) does not exist on the object specified.");
            var pi = GetInfo(value.GetType(), propertyName, PropertyAccessRequired.Set);
            pi.SetValue(value, propertyValue, null);
        }

        /// <summary>
        /// Sets the value of an object's property.
        /// </summary>
        /// <param name="staticType">The static type to set the property of.</param>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <param name="propertyValue">The value to set on the property.</param>
        /// <exception cref="System.ArgumentNullException">The staticType or propertyName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The propertyName specified is invalid and either does not exist or is read only.</exception>
        public static void Set(Type staticType, string propertyName, object propertyValue)
        {
            CheckParameter.IsNotNull(staticType, "staticType");
            CheckParameter.IsNotNullOrEmpty(propertyName, "propertyName");

            if (IsReadOnly(staticType, propertyName))
                throw new InvalidOperationException(
                    "The property name specified ({0}) does not exist on the object specified.");
            var pi = GetInfo(staticType, propertyName, PropertyAccessRequired.Set);
            pi.SetValue(null, propertyValue, null);
        }

        #endregion

        #region | Private Methods |

        /// <summary>
        /// Determines if a given set of property information has the required access.
        /// </summary>
        /// <param name="pi">The PropertyInfo object.</param>
        /// <param name="access">The access to check for.</param>
        /// <returns>A flag to indicate if the PropertyInfo passed has the required type of access.</returns>
        private static bool HasRequiredAccess(PropertyInfo pi, PropertyAccessRequired access)
        {
            return (access == PropertyAccessRequired.Get ? pi.CanRead : pi.CanWrite);
        }

        #endregion

        /// <summary>
        /// Functions to simplify access to indexed property data using Reflection.
        /// </summary>
        public static class Indexer
        {
            #region | GetItem |

            /// <summary>
            /// Gets the value of an item within an object's indexed property.
            /// </summary>
            /// <param name="value">The object to get the indexer of.</param>
            /// <param name="index">The index of the property to get.</param>
            /// <returns>The value of the indexer item, or null.</returns>
            /// <exception cref="System.ArgumentNullException">The value or indexerName parameter is null.</exception>
            /// <exception cref="System.ArgumentException">The indexerName specified is invalid.</exception>
            public static object GetItem(object value, int index)
            {
                return GetItem(value, "Item", index);
            }

            /// <summary>
            /// Gets the value of an item within an object's indexed property.
            /// </summary>
            /// <typeparam name="T">The type of the index item to return.</typeparam>
            /// <param name="value">The object to get the indexer of.</param>
            /// <param name="index">The index of the property to get.</param>
            /// <returns>The value of the indexer item, or null.</returns>
            /// <exception cref="System.ArgumentNullException">The value or indexerName parameter is null.</exception>
            /// <exception cref="System.ArgumentException">The indexerName specified is invalid.</exception>
            public static T GetItem<T>(object value, int index)
            {
                return GetItem<T>(value, "Item", index);
            }

            /// <summary>
            /// Gets the value of an item within an object's indexed property.
            /// </summary>
            /// <param name="value">The object to get the indexer of.</param>
            /// <param name="indexerName">The name of the indexer to get.</param>
            /// <param name="index">The index of the property to get.</param>
            /// <returns>The value of the indexer item, or null.</returns>
            /// <exception cref="System.ArgumentNullException">The value or indexerName parameter is null.</exception>
            /// <exception cref="System.ArgumentException">The indexerName specified is invalid.</exception>
            public static object GetItem(object value, string indexerName, int index)
            {
                CheckParameter.IsNotNull(value, "value");
                CheckParameter.IsNotNullOrEmpty(indexerName, "indexerName");

                var pi = GetInfo(value.GetType(), indexerName, PropertyAccessRequired.Get);
                return pi.GetValue(value, new object[] {index});
            }

            /// <summary>
            /// Gets the value of an item within an object's indexed property.
            /// </summary>
            /// <typeparam name="T">The type of the index item to return.</typeparam>
            /// <param name="value">The object to get the indexer of.</param>
            /// <param name="indexerName">The name of the indexer to get.</param>
            /// <param name="index">The index of the property to get.</param>
            /// <returns>The value of the indexer item, or null.</returns>
            /// <exception cref="System.ArgumentNullException">The value or indexerName parameter is null.</exception>
            /// <exception cref="System.ArgumentException">The indexerName specified is invalid.</exception>
            public static T GetItem<T>(object value, string indexerName, int index)
            {
                CheckParameter.IsNotNull(value, "value");
                CheckParameter.IsNotNullOrEmpty(indexerName, "indexerName");

                var retVal = default(T);
                if (GetItem(value, indexerName, index) is T) retVal = (T) GetItem(value, indexerName, index);
                return retVal;
            }

            #endregion

            #region | SetItem |

            /// <summary>
            /// Sets the value of an object's indexer.
            /// </summary>
            /// <typeparam name="T">The type of the object to set.</typeparam>
            /// <typeparam name="TIndexerItem">The type of the indexer.</typeparam>
            /// <param name="value">The object to set the indexer of.</param>
            /// <param name="indexerValue">The value to set on the indexer.</param>
            /// <param name="index">The index of the indexer to set.</param>
            /// <exception cref="System.ArgumentNullException">The value or indexerName parameter is null.</exception>
            /// <exception cref="System.ArgumentException">The indexerName specified is invalid and either does not exist or is read only.</exception>
            public static void SetItem<T, TIndexerItem>(T value, int index, TIndexerItem indexerValue)
            {
                SetItem(value, "Item", index, indexerValue);
            }

            /// <summary>
            /// Sets the value of an object's indexer.
            /// </summary>
            /// <typeparam name="T">The type of the object to set.</typeparam>
            /// <typeparam name="TIndexerItem">The type of the indexer.</typeparam>
            /// <param name="value">The object to set the indexer of.</param>
            /// <param name="indexerName">The name of the indexer to set.</param>
            /// <param name="indexerValue">The value to set on the indexer.</param>
            /// <param name="index">The index of the indexer to set.</param>
            /// <exception cref="System.ArgumentNullException">The value or indexerName parameter is null.</exception>
            /// <exception cref="System.ArgumentException">The indexerName specified is invalid and either does not exist or is read only.</exception>
            public static void SetItem<T, TIndexerItem>(T value, string indexerName, int index,
                TIndexerItem indexerValue)
            {
                CheckParameter.IsNotNull(value, "value");
                CheckParameter.IsNotNullOrEmpty(indexerName, "indexerName");

                var pi = GetInfo(value.GetType(), indexerName, PropertyAccessRequired.Set);
                pi.SetValue(value, indexerValue, new object[] {index});
            }

            #endregion
        }
    }
}
