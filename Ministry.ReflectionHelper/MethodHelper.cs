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
    /// Functions to simplify access to method data using Reflection.
    /// </summary>
    /// <remarks>
    /// Keith Jackson
    /// 04/02/2011
    /// </remarks>
    public class MethodHelper
    {

        #region | Constants |

        private const string PARAM_Value = "value";
        private const string PARAM_StaticType = "staticType";
        private const string PARAM_Type = "type";

        private const string PARAM_MethodName = "methodName";

        private const string ERR_InvalidMethod = "The method name specified ({0}) does not exist on the object specified.";

        #endregion

        #region | Construction |

        internal MethodHelper() { }

        #endregion

        #region | Public Methods |

        #region | ExecuteMethod |

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <param name="value">The object to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public void ExecuteMethod(object value, string methodName)
        {
            ExecuteMethod(value, methodName, null, null, null);
        }

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <typeparam name="T">The type of the return value from the method.</typeparam>
        /// <param name="value">The object to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <returns>The return value of the method, or null.</returns>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public T ExecuteMethod<T>(object value, string methodName)
        {
            return ExecuteMethod<T>(value, methodName, null, null, null);
        }

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <param name="value">The object to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <param name="methodParameters">Parameters required to execute the method.</param>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public void ExecuteMethod(object value, string methodName, params object[] methodParameters)
        {
            ExecuteMethod(value, methodName, null, ConstructDefaultMethodParameterTypesArray(methodParameters), methodParameters);
        }

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <param name="value">The object to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <param name="methodParameters">Parameters required to execute the method.</param>
        /// <returns>The return value of the method, or null.</returns>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public T ExecuteMethod<T>(object value, string methodName, params object[] methodParameters)
        {
            return ExecuteMethod<T>(value, methodName, null, ConstructDefaultMethodParameterTypesArray(methodParameters), methodParameters);
        }

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <param name="value">The object to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <param name="typeArguments">An array of type arguments for a generic method.</param>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public void ExecuteMethod(object value, string methodName, Type[] typeArguments)
        {
            ExecuteMethod(value, methodName, typeArguments, null, null);
        }

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <typeparam name="T">The type of the method return value.</typeparam>
        /// <param name="value">The object to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <param name="typeArguments">An array of type arguments for a generic method.</param>
        /// <returns>The return value of the method, or null.</returns>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public T ExecuteMethod<T>(object value, string methodName, Type[] typeArguments)
        {
            return ExecuteMethod<T>(value, methodName, typeArguments, null, null);
        }

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <param name="value">The object to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <param name="typeArguments">An array of type arguments for a generic method.</param>
        /// <param name="parameterTypes">An array of type arguments for method parameters.</param>
        /// <param name="methodParameters">Parameters required to execute the method.</param>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public void ExecuteMethod(object value, string methodName, Type[] typeArguments, Type[] parameterTypes, params object[] methodParameters)
        {
            if (value == null) throw new ArgumentNullException(PARAM_Value);
            if (methodName == null) throw new ArgumentNullException(PARAM_MethodName);
            MethodInfo mi = GetMethodInfo(value.GetType(), methodName, typeArguments, parameterTypes);
            mi.Invoke(value, methodParameters);
        }

        /// <summary>
        /// Executes a method on an object.
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
        public T ExecuteMethod<T>(object value, string methodName, Type[] typeArguments, Type[] parameterTypes, params object[] methodParameters)
        {
            if (value == null) throw new ArgumentNullException(PARAM_Value);
            if (methodName == null) throw new ArgumentNullException(PARAM_MethodName);
            MethodInfo mi = GetMethodInfo(value.GetType(), methodName, typeArguments, parameterTypes);
            return (T)mi.Invoke(value, methodParameters);
        }

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <param name="staticType">The type of the class to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public void ExecuteMethod(Type staticType, string methodName)
        {
            ExecuteMethod(staticType, methodName, null, null, null);
        }

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <typeparam name="T">The type of the method return value.</typeparam>
        /// <param name="staticType">The type of the class to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <returns>The return value of the method, or null.</returns>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public T ExecuteMethod<T>(Type staticType, string methodName)
        {
            return ExecuteMethod<T>(staticType, methodName, null, null, null);
        }

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <param name="staticType">The type of the class to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <param name="methodParameters">Parameters required to execute the method.</param>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public void ExecuteMethod(Type staticType, string methodName, params object[] methodParameters)
        {
            ExecuteMethod(staticType, methodName, null, ConstructDefaultMethodParameterTypesArray(methodParameters), methodParameters);
        }

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <typeparam name="T">The type of the method return value.</typeparam>
        /// <param name="staticType">The type of the class to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <param name="methodParameters">Parameters required to execute the method.</param>
        /// <returns>The return value of the method, or null.</returns>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public T ExecuteMethod<T>(Type staticType, string methodName, params object[] methodParameters)
        {
            return ExecuteMethod<T>(staticType, methodName, null, ConstructDefaultMethodParameterTypesArray(methodParameters), methodParameters);
        }

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <param name="staticType">The type of the class to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <param name="typeArguments">An array of type arguments for a generic method.</param>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public void ExecuteMethod(Type staticType, string methodName, Type[] typeArguments)
        {
            ExecuteMethod(staticType, methodName, typeArguments, null, null);
        }

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <typeparam name="T">The type of the method return value.</typeparam>
        /// <param name="staticType">The type of the class to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <param name="typeArguments">An array of type arguments for a generic method.</param>
        /// <returns>The return value of the method, or null.</returns>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public T ExecuteMethod<T>(Type staticType, string methodName, Type[] typeArguments)
        {
            return ExecuteMethod<T>(staticType, methodName, typeArguments, null, null);
        }

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <param name="staticType">The type of the class to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <param name="typeArguments">An array of type arguments for a generic method.</param>
        /// <param name="parameterTypes">An array of type arguments for method parameters.</param>
        /// <param name="methodParameters">Parameters required to execute the method.</param>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public void ExecuteMethod(Type staticType, string methodName, Type[] typeArguments, Type[] parameterTypes, params object[] methodParameters)
        {
            if (staticType == null) throw new ArgumentNullException(PARAM_StaticType);
            if (String.IsNullOrEmpty(methodName)) throw new ArgumentNullException(PARAM_MethodName);
            MethodInfo mi = GetMethodInfo(staticType, methodName, typeArguments, parameterTypes);
            mi.Invoke(null, methodParameters);
        }

        /// <summary>
        /// Executes a method on an object.
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
        public T ExecuteMethod<T>(Type staticType, string methodName, Type[] typeArguments, Type[] parameterTypes, params object[] methodParameters)
        {
            if (staticType == null) throw new ArgumentNullException(PARAM_StaticType);
            if (String.IsNullOrEmpty(methodName)) throw new ArgumentNullException(PARAM_MethodName);
            MethodInfo mi = GetMethodInfo(staticType, methodName, typeArguments, parameterTypes);
            return (T)mi.Invoke(null, methodParameters);
        }

        #endregion

        #region | GetMethodInfo |

        /// <summary>
        /// Gets the details for an object's method.
        /// </summary>
        /// <param name="value">The object to get the method details from.</param>
        /// <param name="methodName">The name of the method to get the details for.</param>
        /// <returns>A MethodInfo object for accessing method information.</returns>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public MethodInfo GetMethodInfo(object value, string methodName)
        {
            return GetMethodInfo(value.GetType(), methodName);
        }

        /// <summary>
        /// Searches recursively through an object tree for method information.
        /// </summary>
        /// <param name="type">The type of the object to get the method from.</param>
        /// <param name="methodName">The name of the method to search for.</param>
        /// <returns>A MethodInfo object for analysing the method data.</returns>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public MethodInfo GetMethodInfo(Type type, string methodName)
        {
            MethodInfo mi = null;

            try
            {
                mi = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);
            }
            catch (AmbiguousMatchException)
            {
                mi = GetMethodInfo(type, methodName, new Type[0]);
            }

            if (mi == null)
            {
                if (type.BaseType != null)
                {
                    mi = GetMethodInfo(type.BaseType, methodName);
                }
                else
                {
                    throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, ERR_InvalidMethod, methodName));
                }
            }
            return mi;
        }

        /// <summary>
        /// Searches recursively through an object tree for method information.
        /// </summary>
        /// <param name="type">The type of the object to get the method from.</param>
        /// <param name="methodName">The name of the method to search for.</param>
        /// <returns>A MethodInfo object for analysing the method data.</returns>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public MethodInfo GetMethodInfo(Type type, string methodName, Type[] parameterTypes)
        {
            MethodInfo mi = null;
            ParameterInfo[] parameters = null;

            MethodInfo[] methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);
            foreach (MethodInfo method in methods)
            {
                if (method.Name == methodName && !method.IsGenericMethod)
                {
                    parameters = method.GetParameters();
                    if (parameters.Length == parameterTypes.Length)
                    {
                        bool found = true;
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            if (parameters[i].ParameterType != parameterTypes[i]) found = false;
                        }
                        if (found)
                        {
                            mi = method;
                            break;
                        }
                    }
                }
            }

            if (mi == null)
            {
                if (type.BaseType != null)
                {
                    mi = GetMethodInfo(type.BaseType, methodName, parameterTypes);
                }
                else
                {
                    throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, ERR_InvalidMethod, methodName));
                }
            }
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
        public MethodInfo GetMethodInfo(Type type, string methodName, Type[] typeArguments, Type[] parameterTypes)
        {
            MethodInfo mi = null;
            ParameterInfo[] parameters = null;

            if (typeArguments == null && parameterTypes == null)
            {
                mi = GetMethodInfo(type, methodName);
            }
            else if (typeArguments == null)
            {
                mi = GetMethodInfo(type, methodName, parameterTypes);
            }
            else
            {
                MethodInfo[] methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);
                foreach (MethodInfo method in methods)
                {
                    if (method.Name == methodName)
                    {
                        if (method.GetGenericArguments().Length == typeArguments.Length)
                        {
                            MethodInfo generic = method.MakeGenericMethod(typeArguments);
                            parameters = generic.GetParameters();
                            if (parameterTypes == null) parameterTypes = new Type[0];
                            if (parameters.Length == parameterTypes.Length)
                            {
                                bool found = true;
                                for (int i = 0; i < parameters.Length; i++)
                                {
                                    if (parameters[i].ParameterType != parameterTypes[i]) found = false;
                                }
                                if (found)
                                {
                                    mi = generic;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (mi == null)
            {
                if (type.BaseType != null)
                {
                    mi = GetMethodInfo(type.BaseType, methodName, typeArguments, parameterTypes);
                }
                else
                {
                    throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, ERR_InvalidMethod, methodName));
                }
            }
            return mi;
        }

        #endregion

        #region | GetMethodParameters |

        /// <summary>
        /// Gets the parameters for an object's method.
        /// </summary>
        /// <param name="value">The object to get the method parameters from.</param>
        /// <param name="methodName">The name of the method to get the parameters for.</param>
        /// <returns>An array of parameter information for the specified method.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Not valid here due to structural constraints")]
        public ParameterInfo[] GetMethodParameters(object value, string methodName)
        {
            MethodInfo mi = value.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);
            if (mi == null) throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, ERR_InvalidMethod, methodName));
            return mi.GetParameters();
        }

        #endregion

        #endregion

        #region | Private Methods |

        /// <summary>
        /// Obtains a default method parameter type set.
        /// </summary>
        /// <param name="methodParameters">The method parameters to build the type set from.</param>
        /// <returns>An array of types to represent the types of the parameters passed into the method.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Not valid here due to structural constraints")]
        private Type[] ConstructDefaultMethodParameterTypesArray(object[] methodParameters)
        {
            Type[] parameterTypes = null;
            if (methodParameters.Length > 0)
            {
                parameterTypes = new Type[methodParameters.Length];
                for (int i = 0; i < methodParameters.Length; i++)
                {
                    parameterTypes[i] = methodParameters[i].GetType();
                }
            }
            return parameterTypes;
        }

        #endregion

    }
}
