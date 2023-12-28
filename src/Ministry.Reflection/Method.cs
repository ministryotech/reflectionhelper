namespace Ministry.Reflection;

/// <summary>
/// Functions to simplify access to method data using Reflection.
/// </summary>
[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Library")]
[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Library")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Library")]
public static class Method
{
    #region | Execute (void) |

    /// <summary>
    /// Executes a method on an object instance that returns nothing.
    /// </summary>
    /// <param name="value">The object to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static void Execute(object value, string methodName)
        => TriggerExecute(value, methodName, null, null, null);

    /// <summary>
    /// Executes a method on an object instance that returns nothing.
    /// </summary>
    /// <param name="value">The object to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <returns>The return value of the method, or null.</returns>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static void Execute(object value, string methodName, params object[] methodParameters)
        => TriggerExecute(value, methodName, null, ConstructDefaultMethodParameterTypesArray(methodParameters), methodParameters);

    /// <summary>
    /// Executes a method on an object instance that returns nothing.
    /// </summary>
    /// <param name="value">The object to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static void Execute(object value, string methodName, params MethodParameter[] methodParameters)
        => TriggerExecute(value, methodName, null, ConstructMethodParameterTypesArrayFromObject(methodParameters),
            ConstructMethodParameterValuesArrayFromObject(methodParameters));

    /// <summary>
    /// Executes a method on an object instance that returns nothing.
    /// </summary>
    /// <param name="value">The object to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="parameterTypes">An array of type arguments for method parameters.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static void Execute(object value, string methodName, Type[] parameterTypes, object[] methodParameters)
        => TriggerExecute(value, methodName, null, parameterTypes, methodParameters);

    #region | Statics |

    /// <summary>
    /// Executes a method on a static object that returns nothing.
    /// </summary>
    /// <param name="staticType">The type of the class to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static void Execute(Type staticType, string methodName)
        => TriggerExecute(staticType, methodName, null, null, null);

    /// <summary>
    /// Executes a method on a static object that returns nothing.
    /// </summary>
    /// <param name="staticType">The type of the class to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static void Execute(Type staticType, string methodName, params object[] methodParameters)
        => TriggerExecute(staticType, methodName, null, ConstructDefaultMethodParameterTypesArray(methodParameters), methodParameters);

    /// <summary>
    /// Executes a method on a static object that returns nothing.
    /// </summary>
    /// <param name="staticType">The type of the class to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static void Execute(Type staticType, string methodName, params MethodParameter[] methodParameters)
        => TriggerExecute(staticType, methodName, null, ConstructMethodParameterTypesArrayFromObject(methodParameters), 
            ConstructMethodParameterValuesArrayFromObject(methodParameters));

    /// <summary>
    /// Executes a method on a static object that returns nothing.
    /// </summary>
    /// <param name="staticType">The type of the class to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="parameterTypes">An array of type arguments for method parameters.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static void Execute(Type staticType, string methodName, Type[] parameterTypes, object[] methodParameters)
        => TriggerExecute(staticType, methodName, null, parameterTypes, methodParameters);

    #endregion

    #endregion

    #region | Execute (T) |

    /// <summary>
    /// Executes a method on an object instance that returns a value.
    /// </summary>
    /// <typeparam name="T">The type of the return value from the method.</typeparam>
    /// <param name="value">The object to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <returns>The return value of the method, or null.</returns>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static T Execute<T>(object value, string methodName)
        => TriggerExecuteAndReturn<T>(value, methodName, null, null, null);

    /// <summary>
    /// Executes a method on an object instance that returns a value.
    /// </summary>
    /// <param name="value">The object to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <returns>The return value of the method, or null.</returns>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static T Execute<T>(object value, string methodName, params object[] methodParameters)
        => TriggerExecuteAndReturn<T>(value, methodName, null, ConstructDefaultMethodParameterTypesArray(methodParameters), methodParameters);

    /// <summary>
    /// Executes a method on an object instance that returns a value.
    /// </summary>
    /// <typeparam name="T">The type of the method return value.</typeparam>
    /// <param name="value">The object to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <returns>The return value of the method, or null.</returns>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static T Execute<T>(object value, string methodName, params MethodParameter[] methodParameters)
        => TriggerExecuteAndReturn<T>(value, methodName, null, ConstructMethodParameterTypesArrayFromObject(methodParameters),
            ConstructMethodParameterValuesArrayFromObject(methodParameters));

    /// <summary>
    /// Executes a method on an object instance that returns a value.
    /// </summary>
    /// <typeparam name="T">The type of the method return value.</typeparam>
    /// <param name="value">The object to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="parameterTypes">An array of type arguments for method parameters.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <returns>The return value of the method, or null.</returns>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static T Execute<T>(object value, string methodName, Type[] parameterTypes, object[] methodParameters)
        => TriggerExecuteAndReturn<T>(value, methodName, null, parameterTypes, methodParameters);

    #region | Statics |

    /// <summary>
    /// Executes a method on a static object that returns a value.
    /// </summary>
    /// <typeparam name="T">The type of the method return value.</typeparam>
    /// <param name="staticType">The type of the class to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <returns>The return value of the method, or null.</returns>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static T Execute<T>(Type staticType, string methodName)
        => TriggerExecuteAndReturn<T>(staticType, methodName, null, null, null);

    /// <summary>
    /// Executes a method on a static object that returns a value.
    /// </summary>
    /// <typeparam name="T">The type of the method return value.</typeparam>
    /// <param name="staticType">The type of the class to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <returns>The return value of the method, or null.</returns>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static T Execute<T>(Type staticType, string methodName, params object[] methodParameters)
        => TriggerExecuteAndReturn<T>(staticType, methodName, null, ConstructDefaultMethodParameterTypesArray(methodParameters), methodParameters);

    /// <summary>
    /// Executes a method on a static object that returns a value.
    /// </summary>
    /// <typeparam name="T">The type of the method return value.</typeparam>
    /// <param name="staticType">The type of the class to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <returns>The return value of the method, or null.</returns>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static T Execute<T>(Type staticType, string methodName, params MethodParameter[] methodParameters)
        => TriggerExecuteAndReturn<T>(staticType, methodName, null, ConstructMethodParameterTypesArrayFromObject(methodParameters),
            ConstructMethodParameterValuesArrayFromObject(methodParameters));

    /// <summary>
    /// Executes a method on a static object that returns a value.
    /// </summary>
    /// <typeparam name="T">The type of the method return value.</typeparam>
    /// <param name="staticType">The type of the class to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="parameterTypes">An array of type arguments for method parameters.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <returns>The return value of the method, or null.</returns>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static T Execute<T>(Type staticType, string methodName, Type[] parameterTypes, object[] methodParameters)
        => TriggerExecuteAndReturn<T>(staticType, methodName, null, parameterTypes, methodParameters);

    #endregion

    #endregion

    #region | Execute (Generic Method - void) |

    /// <summary>
    /// Executes a method on a generic object instance that returns nothing.
    /// </summary>
    /// <param name="value">The object to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="typeArguments">An array of type arguments for a generic method.</param>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static void Execute(object value, string methodName, Type[] typeArguments)
        => TriggerExecute(value, methodName, typeArguments, null, null);

    /// <summary>
    /// Executes a method on a generic object instance that returns nothing.
    /// </summary>
    /// <param name="value">The object to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="typeArguments">An array of type arguments for a generic method.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <returns>The return value of the method, or null.</returns>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static void Execute(object value, string methodName, Type[] typeArguments, params MethodParameter[] methodParameters)
        => TriggerExecute(value, methodName, typeArguments, ConstructMethodParameterTypesArrayFromObject(methodParameters),
            ConstructMethodParameterValuesArrayFromObject(methodParameters));

    /// <summary>
    /// Executes a method on a generic object instance that returns nothing.
    /// </summary>
    /// <param name="value">The object to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="typeArguments">An array of type arguments for a generic method.</param>
    /// <param name="parameterTypes">An array of type arguments for method parameters.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static void Execute(object value, string methodName, Type[] typeArguments, Type[] parameterTypes, object[] methodParameters)
        => TriggerExecute(value, methodName, typeArguments, parameterTypes, methodParameters);

    #region | Statics |

    /// <summary>
    /// Executes a method on a static generic object that returns nothing.
    /// </summary>
    /// <param name="staticType">The type of the class to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="typeArguments">An array of type arguments for a generic method.</param>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static void Execute(Type staticType, string methodName, Type[] typeArguments)
        => TriggerExecute(staticType, methodName, typeArguments, null, null);

    /// <summary>
    /// Executes a method on a static generic object that returns nothing.
    /// </summary>
    /// <param name="staticType">The type of the class to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="typeArguments">An array of type arguments for a generic method.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static void Execute(Type staticType, string methodName, Type[] typeArguments, params MethodParameter[] methodParameters)
        => TriggerExecute(staticType, methodName, typeArguments, ConstructMethodParameterTypesArrayFromObject(methodParameters),
            ConstructMethodParameterValuesArrayFromObject(methodParameters));

    /// <summary>
    /// Executes a method on a static generic object that returns nothing.
    /// </summary>
    /// <param name="staticType">The type of the class to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="typeArguments">An array of type arguments for a generic method.</param>
    /// <param name="parameterTypes">An array of type arguments for method parameters.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static void Execute(Type staticType, string methodName, Type[] typeArguments, Type[] parameterTypes, object[] methodParameters)
        => TriggerExecute(staticType, methodName, typeArguments, parameterTypes, methodParameters);

    #endregion

    #endregion

    #region | Execute (Generic Method - T) |

    /// <summary>
    /// Executes a method on a generic object instance that returns a value.
    /// </summary>
    /// <typeparam name="T">The type of the method return value.</typeparam>
    /// <param name="value">The object to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="typeArguments">An array of type arguments for a generic method.</param>
    /// <returns>The return value of the method, or null.</returns>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static T Execute<T>(object value, string methodName, Type[] typeArguments)
        => TriggerExecuteAndReturn<T>(value, methodName, typeArguments, null, null);

    /// <summary>
    /// Executes a method on a generic object instance that returns a value.
    /// </summary>
    /// <typeparam name="T">The type of the method return value.</typeparam>
    /// <param name="value">The object to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="typeArguments">An array of type arguments for a generic method.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <returns>The return value of the method, or null.</returns>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static T Execute<T>(object value, string methodName, Type[] typeArguments, params MethodParameter[] methodParameters)
        => TriggerExecuteAndReturn<T>(value, methodName, typeArguments, ConstructMethodParameterTypesArrayFromObject(methodParameters),
            ConstructMethodParameterValuesArrayFromObject(methodParameters));

    /// <summary>
    /// Executes a method on a generic object instance that returns a value.
    /// </summary>
    /// <typeparam name="T">The type of the method return value.</typeparam>
    /// <param name="value">The object to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="typeArguments">An array of type arguments for a generic method.</param>
    /// <param name="parameterTypes">An array of type arguments for method parameters.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <returns>The return value of the method, or null.</returns>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static T Execute<T>(object value, string methodName, Type[] typeArguments, Type[] parameterTypes, object[] methodParameters)
        => TriggerExecuteAndReturn<T>(value, methodName, typeArguments, parameterTypes, methodParameters);

    #region | Statics |

    /// <summary>
    /// Executes a method on a static generic object that returns a value.
    /// </summary>
    /// <typeparam name="T">The type of the method return value.</typeparam>
    /// <param name="staticType">The type of the class to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="typeArguments">An array of type arguments for a generic method.</param>
    /// <returns>The return value of the method, or null.</returns>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static T Execute<T>(Type staticType, string methodName, Type[] typeArguments)
        => TriggerExecuteAndReturn<T>(staticType, methodName, typeArguments, null, null);

    /// <summary>
    /// Executes a method on a static generic object that returns a value.
    /// </summary>
    /// <typeparam name="T">The type of the method return value.</typeparam>
    /// <param name="staticType">The type of the class to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="typeArguments">An array of type arguments for a generic method.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <returns>The return value of the method, or null.</returns>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static T Execute<T>(Type staticType, string methodName, Type[] typeArguments, params MethodParameter[] methodParameters)
        => TriggerExecuteAndReturn<T>(staticType, methodName, typeArguments, ConstructMethodParameterTypesArrayFromObject(methodParameters),
            ConstructMethodParameterValuesArrayFromObject(methodParameters));

    /// <summary>
    /// Executes a method on a static generic object that returns a value.
    /// </summary>
    /// <typeparam name="T">The type of the method return value.</typeparam>
    /// <param name="staticType">The type of the class to execute the method from.</param>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="typeArguments">An array of type arguments for a generic method.</param>
    /// <param name="parameterTypes">An array of type arguments for method parameters.</param>
    /// <param name="methodParameters">Parameters required to execute the method.</param>
    /// <returns>The return value of the method, or null.</returns>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static T Execute<T>(Type staticType, string methodName, Type[] typeArguments, Type[] parameterTypes, object[] methodParameters)
        => TriggerExecuteAndReturn<T>(staticType, methodName, typeArguments, parameterTypes, methodParameters);

    #endregion

    #endregion

    #region | GetInfo |

    /// <summary>
    /// Gets the details for an object's method.
    /// </summary>
    /// <param name="value">The object to get the method details from.</param>
    /// <param name="methodName">The name of the method to get the details for.</param>
    /// <returns>A MethodInfo object for accessing method information.</returns>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static MethodInfo GetInfo(object value, string methodName)
        => GetInfo(value.GetType(), methodName);

    /// <summary>
    /// Searches recursively through an object tree for method information.
    /// </summary>
    /// <param name="type">The type of the object to get the method from.</param>
    /// <param name="methodName">The name of the method to search for.</param>
    /// <returns>A MethodInfo object for analysing the method data.</returns>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static MethodInfo GetInfo(Type type, string methodName)
        => GetInfo(type, methodName, Array.Empty<Type>());

    /// <summary>
    /// Searches recursively through an object tree for method information.
    /// </summary>
    /// <param name="type">The type of the object to get the method from.</param>
    /// <param name="methodName">The name of the method to search for.</param>
    /// <param name="parameterTypes">The parameter types.</param>
    /// <returns>
    /// A MethodInfo object for analysing the method data.
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static MethodInfo GetInfo(Type type, string methodName, Type[] parameterTypes)
    {
            MethodInfo mi;

            try
            {
                mi = type.ThrowIfNull(nameof(type))
                    .GetRuntimeMethod(methodName.ThrowIfNullOrEmpty(nameof(methodName)), parameterTypes);
            }
            catch (AmbiguousMatchException)
            {
                mi = GetInfo(type, methodName, Type.EmptyTypes);
            }

            if (mi != null) return mi;

            if (type.GetTypeInfo().BaseType != null)
                mi = GetInfo(type.GetTypeInfo().BaseType, methodName);
            else
                throw new ArgumentException(
                    $"The method name specified ({methodName}) does not exist on the object specified.");

            return mi;
        }        
        
    /// <summary>
    /// Searches recursively through an object tree for method information.
    /// </summary>
    /// <param name="type">The type of the object to get the method from.</param>
    /// <param name="methodName">The name of the method to search for.</param>
    /// <param name="parameterTypes">An array of type arguments for method parameters.</param>
    /// <param name="typeArguments">An array of type arguments for a generic method.</param>
    /// <returns>A MethodInfo object for analysing the method data.</returns>
    /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
    /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
    public static MethodInfo GetInfo(Type type, string methodName, Type[] typeArguments, Type[] parameterTypes)
    {
            MethodInfo mi = null;

            if (typeArguments == null && parameterTypes == null)
                return GetInfo(type, methodName);

            if (typeArguments == null)
                return GetInfo(type, methodName, parameterTypes);

            var methods = type.ThrowIfNull(nameof(type)).GetRuntimeMethods().Where(
                m => m.Name == methodName.ThrowIfNullOrEmpty(nameof(methodName))
                && m.GetGenericArguments().Length == typeArguments.Length);

            foreach (var method in methods)
            {
                var generic = method.MakeGenericMethod(typeArguments);
                var parameters = generic.GetParameters();

                parameterTypes ??= Type.EmptyTypes;
                if (parameters.Length != parameterTypes.Length) continue;
                var found = !parameters.Where((t, i) => !t.ParameterType.GetTypeInfo().IsAssignableFrom(parameterTypes[i])).Any();
                if (!found) continue;
                mi = generic;
                break;
            }

            if (mi != null) return mi;
            if (type.GetTypeInfo().BaseType != null)
                mi = GetInfo(type.GetTypeInfo().BaseType, methodName, parameterTypes);
            else
                throw new ArgumentException($"The method name specified ({methodName}) does not exist on the object specified.");

            return mi;
        }

    #endregion

    #region | GetParameters |

    /// <summary>
    /// Gets the parameters for an object's method.
    /// </summary>
    /// <param name="value">The object to get the method parameters from.</param>
    /// <param name="methodName">The name of the method to get the parameters for.</param>
    /// <returns>An array of parameter information for the specified method.</returns>
    public static ParameterInfo[] GetParameters(object value, string methodName)
    {
            var mi = value.ThrowIfNull(nameof(value)).GetType().GetRuntimeMethods().FirstOrDefault(
                m => m.Name == methodName.ThrowIfNullOrEmpty(nameof(methodName)));

            if (mi == null) throw new InvalidOperationException($"The method name specified ({methodName}) does not exist on the object specified.");
            return mi.GetParameters();
        }

    #endregion

    #region | Private Methods |

    /// <summary>
    /// Obtains a default method parameter type set.
    /// </summary>
    /// <param name="methodParameters">The method parameters to build the type set from.</param>
    /// <returns>An array of types to represent the types of the parameters passed into the method.</returns>
    private static Type[] ConstructDefaultMethodParameterTypesArray(IList<object> methodParameters)
    {
            if (methodParameters.Count <= 0) return null;
            var parameterTypes = new Type[methodParameters.Count];
            for (var i = 0; i < methodParameters.Count; i++)
            {
                if (methodParameters[i] == null)
                    throw new ArgumentNullException(nameof(methodParameters), $"The provided value at position {i} was null. Please use an overload of Execute that explicitly passes the parameter types.");
                
                parameterTypes[i] = methodParameters[i].GetType();
            }
            return parameterTypes;
        }

    /// <summary>
    /// Obtains a default method parameter type set.
    /// </summary>
    /// <param name="methodParameters">The method parameters to build the type set from.</param>
    /// <returns>An array of types to represent the types of the parameters passed into the method.</returns>
    private static Type[] ConstructMethodParameterTypesArrayFromObject(IList<MethodParameter> methodParameters)
    {
            if (methodParameters.Count <= 0) return null;
            var parameterTypes = new Type[methodParameters.Count];
            for (var i = 0; i < methodParameters.Count; i++)
            {
                if (methodParameters[i].Type == null)
                    throw new ArgumentNullException(nameof(methodParameters), $"The provided value Type at position {i} was null.");

                parameterTypes[i] = methodParameters[i].Type;
            }
            return parameterTypes;
        }

    /// <summary>
    /// Obtains a method parameter values set.
    /// </summary>
    /// <param name="methodParameters">The method parameters to build the values set from.</param>
    /// <returns>An array of values to represent the parameters passed into the method.</returns>
    private static object[] ConstructMethodParameterValuesArrayFromObject(IList<MethodParameter> methodParameters)
    {
            if (methodParameters.Count <= 0) return null;
            var parameterValues = new object[methodParameters.Count];
            for (var i = 0; i < methodParameters.Count; i++)
            {
                parameterValues[i] = methodParameters[i].Value;
            }
            return parameterValues;
        }

    /// <summary>
    /// Triggers the execute.
    /// </summary>
    /// <param name="value">The instance.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="typeArguments">The type arguments.</param>
    /// <param name="parameterTypes">The parameter types.</param>
    /// <param name="methodParameters">The method parameters.</param>
    private static void TriggerExecute(object value, string methodName, Type[] typeArguments, Type[] parameterTypes, object[] methodParameters)
        => GetInfo(value.GetType(), methodName, typeArguments, parameterTypes).Invoke(value, methodParameters);

    /// <summary>
    /// Triggers the execute.
    /// </summary>
    /// <param name="staticType">Type of the static.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="typeArguments">The type arguments.</param>
    /// <param name="parameterTypes">The parameter types.</param>
    /// <param name="methodParameters">The method parameters.</param>
    private static void TriggerExecute(Type staticType, string methodName, Type[] typeArguments, Type[] parameterTypes, object[] methodParameters)
        => GetInfo(staticType, methodName, typeArguments, parameterTypes)
            .Invoke(null, methodParameters);

    /// <summary>
    /// Triggers the execute and returns the value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The instance.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="typeArguments">The type arguments.</param>
    /// <param name="parameterTypes">The parameter types.</param>
    /// <param name="methodParameters">The method parameters.</param>
    /// <returns></returns>
    private static T TriggerExecuteAndReturn<T>(object value, string methodName, Type[] typeArguments, Type[] parameterTypes, object[] methodParameters)
        => (T)GetInfo(value.GetType(), methodName, typeArguments, parameterTypes)
            .Invoke(value, methodParameters);

    /// <summary>
    /// Triggers the execute and returns the value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="staticType">Type of the static.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="typeArguments">The type arguments.</param>
    /// <param name="parameterTypes">The parameter types.</param>
    /// <param name="methodParameters">The method parameters.</param>
    /// <returns></returns>
    private static T TriggerExecuteAndReturn<T>(Type staticType, string methodName, Type[] typeArguments, Type[] parameterTypes, object[] methodParameters)
        => (T)GetInfo(staticType, methodName, typeArguments, parameterTypes)
            .Invoke(null, methodParameters);

    #endregion
}