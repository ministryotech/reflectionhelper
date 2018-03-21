using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Ministry.Reflection
{
    /// <summary>
    /// A set of functions to aid reflection
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class Helper
    {
        #region | GetPropertyInfo |

        /// <summary>
        /// Gets the property information.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="this">The source.</param>
        /// <param name="propertyLambda">The property lambda.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// If the provided expression does not return a property.
        /// </exception>
        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(this TSource @this, Expression<Func<TSource, TProperty>> propertyLambda)
        {
            @this.ThrowIfNull(nameof(@this));
            if (!(propertyLambda.ThrowIfNull(nameof(propertyLambda)).Body is MemberExpression member))
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");

            if (!(member.Member is PropertyInfo propInfo))
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");

            return propInfo;
        }

        /// <summary>
        /// Gets the property information.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="this">The source.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo<TSource>(this TSource @this, string propertyName)
        {
            @this.ThrowIfNull(nameof(@this));
            var propInfo = @this.GetType().GetTypeInfo().GetProperty(propertyName.ThrowIfNullOrEmpty(nameof(propertyName)));
            if (propInfo == null)
                throw new ArgumentException($"Name '{propertyName}' not found.");

            return propInfo;
        }

        #endregion

        #region | GetPropertyValue |

        /// <summary>
        /// Gets the value of a property.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="this">The source.</param>
        /// <param name="propertyLambda">The property lambda.</param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        public static TProperty GetPropertyValue<TSource, TProperty>(this TSource @this, Expression<Func<TSource, TProperty>> propertyLambda)
            => (TProperty) @this.GetPropertyInfo(propertyLambda).GetMethod.Invoke(@this, new object[0]);

        /// <summary>
        /// Gets the value of a property.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="this">The source.</param>
        /// <param name="propertyLambda">The property lambda.</param>
        /// <param name="defaultValue">The default value to return if no value is found.</param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        public static TProperty GetPropertyValue<TSource, TProperty>(this TSource @this, Expression<Func<TSource, TProperty>> propertyLambda, 
            TProperty defaultValue)
            => (TProperty)(@this.GetPropertyInfo(propertyLambda).GetMethod.Invoke(@this, new object[0]) ?? defaultValue);

        /// <summary>
        /// Gets the value of a property.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="this">The source.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="defaultValue">The default value to return if no value is found.</param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        public static TProperty GetPropertyValue<TSource, TProperty>(this TSource @this, string propertyName, TProperty defaultValue)
            => (TProperty)(@this.GetPropertyValueForUnknownType(propertyName, defaultValue));

        /// <summary>
        /// Gets the value of a property.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="this">The source.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="defaultValue">The default value to return if no value is found.</param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        public static object GetPropertyValueForUnknownType<TSource>(this TSource @this, string propertyName, object defaultValue = null)
            => @this.GetPropertyInfo(propertyName).GetMethod.Invoke(@this, new object[0]) ?? defaultValue;

        #endregion

        #region | HasAttribute |

        /// <summary>
        /// Determines whether the specified class type has an attribute.
        /// </summary>
        /// <typeparam name="TAttributeType">The type of the attribute type.</typeparam>
        /// <param name="classType">Type of the class.</param>
        /// <exception cref="ArgumentNullException">The classType parameter is null.</exception>
        public static bool HasAttribute<TAttributeType>(this Type classType)
            where TAttributeType : Attribute
            => classType.ThrowIfNull(nameof(classType)).GetTypeInfo().GetCustomAttribute<TAttributeType>() != null;

        /// <summary>
        /// Determines whether the specified class type has an attribute.
        /// </summary>
        /// <typeparam name="TAttributeType">The type of the attribute type.</typeparam>
        /// <param name="classType">Type of the class.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <exception cref="ArgumentException">The methodName is empty.</exception>
        /// <exception cref="ArgumentNullException">The classType or methodName parameters are null.</exception>
        public static bool HasAttribute<TAttributeType>(this Type classType, string methodName)
            where TAttributeType : Attribute
        {
            var attribute = classType.ThrowIfNull(nameof(classType)).GetTypeInfo()
                .GetCustomAttribute<TAttributeType>();

            attribute = attribute ??
                classType.GetTypeInfo().GetMethod(methodName.ThrowIfNullOrEmpty(nameof(methodName)))
                .GetCustomAttribute<TAttributeType>();

            return attribute != null;
        }

        /// <summary>
        /// Determines whether the specified class type has an attribute.
        /// </summary>
        /// <typeparam name="TAttributeType">The type of the attribute type.</typeparam>
        /// <param name="classType">Type of the class.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <exception cref="ArgumentException">The methodName is empty.</exception>
        /// <exception cref="ArgumentNullException">The classType, methodName or parameterTypes parameters are null.</exception>
        public static bool HasAttribute<TAttributeType>(this Type classType, string methodName, Type[] parameterTypes)
            where TAttributeType : Attribute
        {
            var attribute = classType.ThrowIfNull(nameof(classType)).GetTypeInfo()
                .GetCustomAttribute<TAttributeType>();

            attribute = attribute ??
                classType.GetTypeInfo().GetMethod(methodName.ThrowIfNullOrEmpty(nameof(methodName)), parameterTypes.ThrowIfNull(nameof(parameterTypes)))
                .GetCustomAttribute<TAttributeType>();

            return attribute != null;
        }

        /// <summary>
        /// Determines whether the specified class type has an attribute on a property.
        /// </summary>
        /// <typeparam name="TAttributeType">The type of the attribute type.</typeparam>
        /// <param name="classType">Type of the class.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <exception cref="ArgumentException">The propertyName is empty.</exception>
        /// <exception cref="ArgumentNullException">The classType or propertyName parameters are null.</exception>
        public static bool HasPropertyAttribute<TAttributeType>(this Type classType, string propertyName)
            where TAttributeType : Attribute
            =>
                classType.ThrowIfNull(nameof(classType)).GetTypeInfo()
                    .GetProperty(propertyName.ThrowIfNullOrEmpty(nameof(propertyName)))
                    .GetCustomAttribute<TAttributeType>() != null;

        /// <summary>
        /// Determines whether the specified class type has a default value attribute on a property.
        /// </summary>
        /// <param name="classType">Type of the class.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <exception cref="ArgumentException">The propertyName is empty.</exception>
        /// <exception cref="ArgumentNullException">The classType, propertyName or expectedValue parameters are null.</exception>
        public static bool HasDefaultValuePropertyAttribute(this Type classType, string propertyName, object expectedValue)
        {
            var attribute =
                classType.ThrowIfNull(nameof(classType)).GetTypeInfo()
                    .GetProperty(propertyName.ThrowIfNullOrEmpty(nameof(propertyName)))
                    .GetCustomAttribute<DefaultValueAttribute>();
            return attribute != null && expectedValue.ThrowIfNull(nameof(expectedValue)).Equals(attribute.Value);
        }

        #endregion
    }
}
