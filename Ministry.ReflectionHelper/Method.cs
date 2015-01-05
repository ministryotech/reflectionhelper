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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Globalization;
using Ministry.StrongTyped;

namespace Ministry.ReflectionHelper
{
    /// <summary>
    /// Functions to simplify access to method data using Reflection.
    /// </summary>
    public static class Method
    {
        #region | Execute |

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <param name="value">The object to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public static void Execute(object value, string methodName)
        {
            Execute(value, methodName, null, null, null);
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
        public static T Execute<T>(object value, string methodName)
        {
            return Execute<T>(value, methodName, null, null, null);
        }

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <param name="value">The object to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <param name="methodParameters">Parameters required to execute the method.</param>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public static void Execute(object value, string methodName, params object[] methodParameters)
        {
            Execute(value, methodName, null, ConstructDefaultMethodParameterTypesArray(methodParameters), methodParameters);
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
        public static T Execute<T>(object value, string methodName, params object[] methodParameters)
        {
            return Execute<T>(value, methodName, null, ConstructDefaultMethodParameterTypesArray(methodParameters), methodParameters);
        }

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <param name="value">The object to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <param name="typeArguments">An array of type arguments for a generic method.</param>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public static void Execute(object value, string methodName, Type[] typeArguments)
        {
            Execute(value, methodName, typeArguments, null, null);
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
        public static T Execute<T>(object value, string methodName, Type[] typeArguments)
        {
            return Execute<T>(value, methodName, typeArguments, null, null);
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
        public static void Execute(object value, string methodName, Type[] typeArguments, Type[] parameterTypes, params object[] methodParameters)
        {
            CheckParameter.IsNotNull(value, "value");
            CheckParameter.IsNotNullOrEmpty(methodName, "methodName");

            var mi = GetInfo(value.GetType(), methodName, typeArguments, parameterTypes);
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
        public static T Execute<T>(object value, string methodName, Type[] typeArguments, Type[] parameterTypes, params object[] methodParameters)
        {
            CheckParameter.IsNotNull(value, "value");
            CheckParameter.IsNotNullOrEmpty(methodName, "methodName");

            var mi = GetInfo(value.GetType(), methodName, typeArguments, parameterTypes);
            return (T)mi.Invoke(value, methodParameters);
        }

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <param name="staticType">The type of the class to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public static void Execute(Type staticType, string methodName)
        {
            Execute(staticType, methodName, null, null, null);
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
        public static T Execute<T>(Type staticType, string methodName)
        {
            return Execute<T>(staticType, methodName, null, null, null);
        }

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <param name="staticType">The type of the class to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <param name="methodParameters">Parameters required to execute the method.</param>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public static void Execute(Type staticType, string methodName, params object[] methodParameters)
        {
            Execute(staticType, methodName, null, ConstructDefaultMethodParameterTypesArray(methodParameters), methodParameters);
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
        public static T Execute<T>(Type staticType, string methodName, params object[] methodParameters)
        {
            return Execute<T>(staticType, methodName, null, ConstructDefaultMethodParameterTypesArray(methodParameters), methodParameters);
        }

        /// <summary>
        /// Executes a method on an object.
        /// </summary>
        /// <param name="staticType">The type of the class to execute the method from.</param>
        /// <param name="methodName">The name of the method to execute.</param>
        /// <param name="typeArguments">An array of type arguments for a generic method.</param>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public static void Execute(Type staticType, string methodName, Type[] typeArguments)
        {
            Execute(staticType, methodName, typeArguments, null, null);
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
        public static T Execute<T>(Type staticType, string methodName, Type[] typeArguments)
        {
            return Execute<T>(staticType, methodName, typeArguments, null, null);
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
        public static void Execute(Type staticType, string methodName, Type[] typeArguments, Type[] parameterTypes, params object[] methodParameters)
        {
            CheckParameter.IsNotNull(staticType, "staticType");
            CheckParameter.IsNotNullOrEmpty(methodName, "methodName");

            var mi = GetInfo(staticType, methodName, typeArguments, parameterTypes);
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
        public static T Execute<T>(Type staticType, string methodName, Type[] typeArguments, Type[] parameterTypes, params object[] methodParameters)
        {
            CheckParameter.IsNotNull(staticType, "staticType");
            CheckParameter.IsNotNullOrEmpty(methodName, "methodName");

            var mi = GetInfo(staticType, methodName, typeArguments, parameterTypes);
            return (T)mi.Invoke(null, methodParameters);
        }

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
        {
            return GetInfo(value.GetType(), methodName);
        }

        /// <summary>
        /// Searches recursively through an object tree for method information.
        /// </summary>
        /// <param name="type">The type of the object to get the method from.</param>
        /// <param name="methodName">The name of the method to search for.</param>
        /// <returns>A MethodInfo object for analysing the method data.</returns>
        /// <exception cref="System.ArgumentException">The name of the method provided is invalid.</exception>
        /// <exception cref="System.ArgumentNullException">The staticType argument is null or the methodName argument is null or empty.</exception>
        public static MethodInfo GetInfo(Type type, string methodName)
        {
            CheckParameter.IsNotNull(type, "type");
            CheckParameter.IsNotNullOrEmpty(methodName, "methodName");

            MethodInfo mi;

            try
            {
                mi = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);
            }
            catch (AmbiguousMatchException)
            {
                mi = GetInfo(type, methodName, new Type[0]);
            }

            if (mi != null) return mi;

            if (type.BaseType != null)
            {
                mi = GetInfo(type.BaseType, methodName);
            }
            else
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The method name specified ({0}) does not exist on the object specified.", methodName));
            }

            return mi;
        }

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
            MethodInfo mi = null;

            var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);
            
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var method in methods)
            {
                if (method.Name != methodName || method.IsGenericMethod) continue;
                var parameters = method.GetParameters();
                if (parameters.Length != parameterTypes.Length) continue;
                var found = !parameters.Where((t, i) => !t.ParameterType.IsAssignableFrom(parameterTypes[i])).Any();
                if (!found) continue;
                mi = method;
                break;
            }

            if (mi != null) return mi;
            if (type.BaseType != null)
            {
                mi = GetInfo(type.BaseType, methodName, parameterTypes);
            }
            else
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The method name specified ({0}) does not exist on the object specified.", methodName));
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
        public static MethodInfo GetInfo(Type type, string methodName, Type[] typeArguments, Type[] parameterTypes)
        {
            MethodInfo mi = null;

            if (typeArguments == null && parameterTypes == null)
            {
                mi = GetInfo(type, methodName);
            }
            else if (typeArguments == null)
            {
                mi = GetInfo(type, methodName, parameterTypes);
            }
            else
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);
                foreach (var method in methods)
                {
                    if (method.Name != methodName) continue;
                    if (method.GetGenericArguments().Length != typeArguments.Length) continue;
                    var generic = method.MakeGenericMethod(typeArguments);
                    var parameters = generic.GetParameters();
                    if (parameterTypes == null) parameterTypes = new Type[0];
                    if (parameters.Length != parameterTypes.Length) continue;
                    var found = !parameters.Where((t, i) => !t.ParameterType.IsAssignableFrom(parameterTypes[i])).Any();
                    if (!found) continue;
                    mi = generic;
                    break;
                }
            }

            if (mi != null) return mi;
            if (type.BaseType != null)
            {
                mi = GetInfo(type.BaseType, methodName, typeArguments, parameterTypes);
            }
            else
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The method name specified ({0}) does not exist on the object specified.", methodName));
            }
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
            var mi = value.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Static);
            if (mi == null) throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "The method name specified ({0}) does not exist on the object specified.", methodName));
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
                parameterTypes[i] = methodParameters[i].GetType();
            }
            return parameterTypes;
        }

        #endregion
    }
}
