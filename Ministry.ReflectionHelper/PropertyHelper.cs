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
using System.Diagnostics.CodeAnalysis;

namespace Ministry.Reflection
{
    /// <summary>
    /// Functions to simplify access to property data using Reflection.
    /// </summary>
    /// <remarks>
    /// Keith Jackson
    /// 04/02/2011
    /// </remarks>
    public class PropertyHelper
    {

        #region | Constants |

        private const string DEF_IndexerName = "Item";

        private const string PARAM_Value = "value";
        private const string PARAM_StaticType = "staticType";
        private const string PARAM_Type = "type";
        private const string PARAM_PropertyName = "propertyName";

        private const string ERR_InvalidProperty = "The property name specified ({0}) does not exist on the object specified.";
        private const string ERR_ReadOnlyProperty = "The property name specified ({0}) is read only.";

        #endregion

        #region | Construction |

        internal PropertyHelper() { }

        #endregion

        #region | Public Methods |

        #region | GetIndexerItem |

        /// <summary>
        /// Gets the value of an item within an object's indexed property.
        /// </summary>
        /// <param name="value">The object to get the indexer of.</param>
        /// <param name="index">The index of the property to get.</param>
        /// <returns>The value of the indexer item, or null.</returns>
        /// <exception cref="System.ArgumentNullException">The value or indexerName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The indexerName specified is invalid.</exception>
        public object GetIndexerItem(object value, int index)
        {
            return GetIndexerItem(value, DEF_IndexerName, index);
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
        public T GetIndexerItem<T>(object value, int index)
        {
            return GetIndexerItem<T>(value, DEF_IndexerName, index);
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
        public object GetIndexerItem(object value, string indexerName, int index)
        {
            if (value == null) throw new ArgumentNullException(PARAM_Value);
            PropertyInfo pi = GetPropertyInfo(value.GetType(), indexerName, PropertyAccessRequired.Get);
            return pi.GetValue(value, new object[] { index });
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
        public T GetIndexerItem<T>(object value, string indexerName, int index)
        {
            T retVal = default(T);
            if (GetIndexerItem(value, indexerName, index) is T) retVal = (T)GetIndexerItem(value, indexerName, index);
            return retVal;
        }

        #endregion

        #region | GetProperty |

        /// <summary>
        /// Gets the value of an object's property.
        /// </summary>
        /// <param name="value">The object to get the property of.</param>
        /// <param name="propertyName">The name of the property to get.</param>
        /// <returns>The value of the property, or null.</returns>
        /// <exception cref="System.ArgumentNullException">The value or propertyName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The propertyName specified is invalid.</exception>
        public object GetProperty(object value, string propertyName)
        {
            if (value == null) throw new ArgumentNullException(PARAM_Value);
            PropertyInfo pi = GetPropertyInfo(value.GetType(), propertyName, PropertyAccessRequired.Get);
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
        public T GetProperty<T>(object value, string propertyName)
        {
            T retVal = default(T);
            if (GetProperty(value, propertyName) is T) retVal = (T)GetProperty(value, propertyName);
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
        public object GetProperty(Type staticType, string propertyName)
        {
            if (staticType == null) throw new ArgumentNullException(PARAM_StaticType);
            PropertyInfo pi = GetPropertyInfo(staticType, propertyName, PropertyAccessRequired.Get);
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
        public T GetProperty<T>(Type staticType, string propertyName)
        {
            T retVal = default(T);
            if (GetProperty(staticType, propertyName) is T) retVal = (T)GetProperty(staticType, propertyName);
            return retVal;
        }

        #endregion

        #region | GetPropertyInfo |

        /// <summary>
        /// Searches recursively through an object tree for property information.
        /// </summary>
        /// <param name="value">The object to get the property from.</param>
        /// <param name="propertyName">The name of the property to search for.</param>
        /// <returns>A PropertyInfo object for analysing the property data.</returns>
        /// <exception cref="System.ArgumentNullException">The type or propertyName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The propertyName specified is invalid.</exception>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Not valid here due to structural constraints")]
        public PropertyInfo GetPropertyInfo(object value, string propertyName)
        {
            if (value == null) throw new ArgumentNullException(PARAM_Value);
            return GetPropertyInfo(value.GetType(), propertyName);
        }

        /// <summary>
        /// Searches recursively through an object tree for property information.
        /// </summary>
        /// <param name="type">The type of the object to get the property from.</param>
        /// <param name="propertyName">The name of the property to search for.</param>
        /// <returns>A PropertyInfo object for analysing the property data.</returns>
        /// <exception cref="System.ArgumentNullException">The type or propertyName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The propertyName specified is invalid.</exception>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Not valid here due to structural constraints")]
        public PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            return GetPropertyInfo(type, propertyName, PropertyAccessRequired.Get, false);
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
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Not valid here due to structural constraints")]
        public PropertyInfo GetPropertyInfo(Type type, string propertyName, PropertyAccessRequired access)
        {
            return GetPropertyInfo(type, propertyName, access, false);
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
        private static PropertyInfo GetPropertyInfo(Type type, string propertyName, PropertyAccessRequired access, bool suppressExceptions)
        {
            if (type == null) throw new ArgumentNullException(PARAM_Type);
            if (propertyName == null) throw new ArgumentNullException(PARAM_PropertyName);
            PropertyInfo pi = type.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);
            bool hasNoAccess = (pi != null ? !HasRequiredAccess(pi, access) : false);
            if (pi == null || hasNoAccess)
            {
                if (type.BaseType != null)
                {
                    pi = GetPropertyInfo(type.BaseType, propertyName, access, suppressExceptions);
                }
                else if (!suppressExceptions)
                {
                    throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, ERR_InvalidProperty, propertyName));
                }
            }

            return pi;
        }

        #endregion

        #region | PropertyExists |

        /// <summary>
        /// Confirms the existence of a property on an object.
        /// </summary>
        /// <typeparam name="T">The type of the object to set.</typeparam>
        /// <param name="value">The object to look for a property on.</param>
        /// <param name="propertyName">The name of the property to look for.</param>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Not valid here due to structural constraints")]
        public bool PropertyExists<T>(T value, string propertyName)
        {
            if (value == null) throw new ArgumentNullException(PARAM_Value);
            PropertyInfo pi = GetPropertyInfo(value.GetType(), propertyName, PropertyAccessRequired.Get, true);
            if (pi == null) pi = GetPropertyInfo(value.GetType(), propertyName, PropertyAccessRequired.Set, true);
            return (pi != null);
        }

        /// <summary>
        /// Confirms the existence of a property on an object.
        /// </summary>
        /// <param name="staticType">The type to look for a property on.</param>
        /// <param name="propertyName">The name of the property to look for.</param>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Not valid here due to structural constraints")]
        public bool PropertyExists(Type staticType, string propertyName)
        {
            PropertyInfo fi = GetPropertyInfo(staticType, propertyName, PropertyAccessRequired.Get, true);
            if (fi == null) fi = GetPropertyInfo(staticType, propertyName, PropertyAccessRequired.Set, true);
            return (fi != null);
        }

        #endregion

        #region | PropertyIsReadOnly |

        /// <summary>
        /// Determines if a property is read only.
        /// </summary>
        /// <param name="value">The object to get the property of.</param>
        /// <param name="propertyName">The name of the property to get.</param>
        /// <returns>A flag to indicate if a property is read only.</returns>
        public bool PropertyIsReadOnly(object value, string propertyName)
        {
            if (value == null) throw new ArgumentNullException(PARAM_Value);
            PropertyInfo pi = GetPropertyInfo(value.GetType(), propertyName, PropertyAccessRequired.Get);
            return !HasRequiredAccess(pi, PropertyAccessRequired.Set);
        }

        /// <summary>
        /// Determines if a property is read only.
        /// </summary>
        /// <param name="staticType">The static type to get the property of.</param>
        /// <param name="propertyName">The name of the property to get.</param>
        /// <returns>A flag to indicate if a property is read only.</returns>
        public bool PropertyIsReadOnly(Type staticType, string propertyName)
        {
            PropertyInfo pi = GetPropertyInfo(staticType, propertyName, PropertyAccessRequired.Get);
            return !HasRequiredAccess(pi, PropertyAccessRequired.Set);
        }

        #endregion

        #region | SetIndexerItem |

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
        public void SetIndexerItem<T, TIndexerItem>(T value, int index, TIndexerItem indexerValue)
        {
            SetIndexerItem<T, TIndexerItem>(value, DEF_IndexerName, index, indexerValue);
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
        public void SetIndexerItem<T, TIndexerItem>(T value, string indexerName, int index, TIndexerItem indexerValue)
        {
            if (value == null) throw new ArgumentNullException(PARAM_Value);
            PropertyInfo pi = GetPropertyInfo(value.GetType(), indexerName, PropertyAccessRequired.Set);
            pi.SetValue(value, indexerValue, new object[] { index });
        }

        #endregion

        #region | SetProperty |

        /// <summary>
        /// Sets the value of an object's property.
        /// </summary>
        /// <param name="value">The object to set the property of.</param>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <param name="propertyValue">The value to set on the property.</param>
        /// <exception cref="System.ArgumentNullException">The value or propertyName parameter is null.</exception>
        /// <exception cref="System.ArgumentException">The propertyName specified is invalid and either does not exist or is read only.</exception>
        public void SetProperty(object value, string propertyName, object propertyValue)
        {
            if (value == null) throw new ArgumentNullException(PARAM_Value);
            if (PropertyIsReadOnly(value, propertyName)) throw new InvalidOperationException(ERR_InvalidProperty);
            PropertyInfo pi = GetPropertyInfo(value.GetType(), propertyName, PropertyAccessRequired.Set);
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
        public void SetProperty(Type staticType, string propertyName, object propertyValue)
        {
            if (staticType == null) throw new ArgumentNullException(PARAM_StaticType);
            if (PropertyIsReadOnly(staticType, propertyName)) throw new InvalidOperationException(ERR_InvalidProperty);
            PropertyInfo pi = GetPropertyInfo(staticType, propertyName, PropertyAccessRequired.Set);
            pi.SetValue(null, propertyValue, null);
        }

        #endregion

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

    }
}
