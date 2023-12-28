namespace Ministry.Reflection;

/// <summary>
/// Functions to simplify access to property data using Reflection.
/// </summary>
[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Library")]
[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Library")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Library")]
#pragma warning disable CA1716
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
        => GetInfo(value.ThrowIfNull(nameof(value)).GetType(),
                propertyName.ThrowIfNullOrEmpty(nameof(propertyName)))
            .GetValue(value, null);

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
        => Get(value.ThrowIfNull(nameof(value)), propertyName.ThrowIfNullOrEmpty(nameof(propertyName))) is T
            ? (T)Get(value, propertyName)
            : default(T);

    /// <summary>
    /// Gets the value of an object's property.
    /// </summary>
    /// <param name="staticType">The static type to get the property of.</param>
    /// <param name="propertyName">The name of the property to get.</param>
    /// <returns>The value of the property, or null.</returns>
    /// <exception cref="System.ArgumentNullException">The staticType or propertyName parameter is null.</exception>
    /// <exception cref="System.ArgumentException">The propertyName specified is invalid.</exception>
    public static object Get(Type staticType, string propertyName)
        => GetInfo(staticType.ThrowIfNull(nameof(staticType)),
                propertyName.ThrowIfNullOrEmpty(nameof(propertyName)))
            .GetValue(null, null);

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
        => Get(staticType.ThrowIfNull(nameof(staticType)), propertyName.ThrowIfNullOrEmpty(nameof(propertyName))) is T
            ? (T)Get(staticType, propertyName)
            : default(T);

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
        => GetInfo(value.ThrowIfNull(nameof(value)).GetType(), 
            propertyName.ThrowIfNullOrEmpty(nameof(propertyName)));

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
    private static PropertyInfo GetInfo(Type type, string propertyName, 
        PropertyAccessRequired access = PropertyAccessRequired.Get,
        bool suppressExceptions = false)
    {
            var pi = type.ThrowIfNull(nameof(type)).GetTypeInfo().GetProperty(propertyName.ThrowIfNullOrEmpty(nameof(propertyName)),
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly |
                BindingFlags.Static);
            var hasNoAccess = pi != null && !HasRequiredAccess(pi, access);
            if (pi != null && !hasNoAccess) return pi;
            if (type.GetTypeInfo().BaseType != null)
            {
                pi = GetInfo(type.GetTypeInfo().BaseType, propertyName, access, suppressExceptions);
            }
            else if (!suppressExceptions)
            {
                throw new ArgumentException($"The property name specified ({propertyName}) does not exist on the object specified.");
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
            value.ThrowIfNull(nameof(value));
            propertyName.ThrowIfNullOrEmpty(nameof(propertyName));

            var pi = GetInfo(value.GetType(), propertyName, PropertyAccessRequired.Get, true) ??
                     GetInfo(value.GetType(), propertyName, PropertyAccessRequired.Set, true);
            return pi != null;
        }

    /// <summary>
    /// Confirms the existence of a property on an object.
    /// </summary>
    /// <param name="staticType">The type to look for a property on.</param>
    /// <param name="propertyName">The name of the property to look for.</param>
    public static bool Exists(Type staticType, string propertyName)
    {
            staticType.ThrowIfNull(nameof(staticType));
            propertyName.ThrowIfNullOrEmpty(nameof(propertyName));

            var pi = GetInfo(staticType, propertyName, PropertyAccessRequired.Get, true) ??
                     GetInfo(staticType, propertyName, PropertyAccessRequired.Set, true);
            return pi != null;
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
        => !HasRequiredAccess(GetInfo(value.GetType(), propertyName), PropertyAccessRequired.Set);

    /// <summary>
    /// Determines if a property is read only.
    /// </summary>
    /// <param name="staticType">The static type to get the property of.</param>
    /// <param name="propertyName">The name of the property to get.</param>
    /// <returns>A flag to indicate if a property is read only.</returns>
    public static bool IsReadOnly(Type staticType, string propertyName)
        => !HasRequiredAccess(GetInfo(staticType, propertyName), PropertyAccessRequired.Set);

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
            if (IsReadOnly(value.ThrowIfNull(nameof(value)), propertyName.ThrowIfNullOrEmpty(nameof(propertyName))))
                throw new InvalidOperationException($"The property name specified ({propertyName}) does not exist on the object specified.");

            GetInfo(value.GetType(), propertyName, PropertyAccessRequired.Set)
                .SetValue(value, propertyValue, null);
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
            if (IsReadOnly(staticType.ThrowIfNull(nameof(staticType)), propertyName.ThrowIfNullOrEmpty(nameof(propertyName))))
                throw new InvalidOperationException($"The property name specified ({propertyName}) does not exist on the object specified.");

            GetInfo(staticType, propertyName, PropertyAccessRequired.Set)
                .SetValue(null, propertyValue, null);
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
        => access == PropertyAccessRequired.Get ? pi.CanRead : pi.CanWrite;

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
            => GetItem(value, "Item", index);

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
            => GetItem<T>(value, "Item", index);

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
            => GetInfo(value.ThrowIfNull(nameof(value)).GetType(),
                    indexerName.ThrowIfNullOrEmpty(nameof(indexerName)))
                .GetValue(value, new object[] {index});

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
            => GetItem(value.ThrowIfNull(nameof(value)), indexerName.ThrowIfNullOrEmpty(nameof(indexerName)), index) is T
                ? (T) GetItem(value, indexerName, index)
                : default(T);

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
            => SetItem(value, "Item", index, indexerValue);

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
        public static void SetItem<T, TIndexerItem>(T value, string indexerName, int index, TIndexerItem indexerValue)
            => GetInfo(value.ThrowIfNull(nameof(value)).GetType(),
                    indexerName.ThrowIfNullOrEmpty(indexerName), PropertyAccessRequired.Set)
                .SetValue(value, indexerValue, new object[] {index});

        #endregion
    }
}
#pragma warning restore CA1716