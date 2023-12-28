namespace Ministry.Reflection;

/// <summary>
/// Functions to simplify access to field data using Reflection.
/// </summary>
[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Library")]
[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Library")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Library")]
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
        => GetInfo(value.ThrowIfNull(nameof(value)).GetType(), 
            fieldName!.ThrowIfNullOrEmpty(nameof(fieldName)), true) != null;

    /// <summary>
    /// Confirms the existence of a field on an object.
    /// </summary>
    /// <param name="staticType">The type to look for a field on.</param>
    /// <param name="fieldName">The name of the field to look for.</param>
    public static bool Exists(Type staticType, string fieldName)
        => GetInfo(staticType.ThrowIfNull(nameof(staticType)),
            fieldName.ThrowIfNullOrEmpty(nameof(fieldName)), true) != null;

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
        => GetInfo(value.ThrowIfNull(nameof(value)).GetType(),
            fieldName.ThrowIfNullOrEmpty(nameof(fieldName)), true).GetValue(value);

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
        => Get(value.ThrowIfNull(nameof(value)), fieldName.ThrowIfNullOrEmpty(nameof(fieldName))) is T 
            ? (T)Get(value, fieldName) 
            : default(T);

    /// <summary>
    /// Gets the value of an object's field.
    /// </summary>
    /// <param name="staticType">The static type to get the field of.</param>
    /// <param name="fieldName">The name of the field to get.</param>
    /// <returns>The value of the field, or null.</returns>
    /// <exception cref="System.ArgumentNullException">The staticType or fieldName parameter is null.</exception>
    /// <exception cref="System.ArgumentException">The fieldName specified is invalid.</exception>
    public static object Get(Type staticType, string fieldName)
        => GetInfo(staticType.ThrowIfNull(nameof(staticType)),
            fieldName.ThrowIfNullOrEmpty(nameof(fieldName)), true).GetValue(null);

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
        => Get(staticType.ThrowIfNull(nameof(staticType)), fieldName.ThrowIfNullOrEmpty(nameof(fieldName))) is T
            ? (T)Get(staticType, fieldName)
            : default(T);

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
        => GetInfo(value.ThrowIfNull(nameof(value)).GetType(), 
            fieldName.ThrowIfNullOrEmpty(nameof(fieldName)));

    /// <summary>
    /// Searches recursively through an object tree for field information.
    /// </summary>
    /// <param name="type">The type of the object to get the field from.</param>
    /// <param name="fieldName">The name of the field to search for.</param>
    /// <param name="suppressExceptions">Specifies that exceptions for invalid fields should be suppressed.</param>
    /// <returns>A FieldInfo object for analysing the field data.</returns>
    /// <exception cref="System.ArgumentNullException">The type or fieldName parameter is null.</exception>
    /// <exception cref="System.ArgumentException">The fieldName specified is invalid.</exception>
    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    private static FieldInfo GetInfo(Type type, string fieldName, bool suppressExceptions = false)
    {
            var fi = type.GetRuntimeField(fieldName);

            if (fi != null) return fi;

            if (type.GetTypeInfo().BaseType != null)
                fi = GetInfo(type.GetTypeInfo().BaseType, fieldName, suppressExceptions);
            else if (!suppressExceptions)
                throw new ArgumentException($"The field name specified ({fieldName}) does not exist on the object specified.", nameof(fieldName));

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
        => GetInfo(value.ThrowIfNull(nameof(value)).GetType(), 
                fieldName.ThrowIfNullOrEmpty(nameof(fieldName)))
            .SetValue(value, fieldValue);

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
        => GetInfo(staticType.ThrowIfNull(nameof(staticType)),
                fieldName.ThrowIfNullOrEmpty(nameof(fieldName)))
            .SetValue(null, fieldValue);

    #endregion
}