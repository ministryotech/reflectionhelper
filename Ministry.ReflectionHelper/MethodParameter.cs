using System;

namespace Ministry.ReflectionHelper
{
    /// <summary>
    /// Represents a method parameter.
    /// </summary>
    public class MethodParameter
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public Type Type { get; set; }
    }
}
