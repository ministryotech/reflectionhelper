namespace Ministry.Reflection;

/// <summary>
/// Specify the property access.
/// </summary>
public enum PropertyAccessRequired
{ 
    /// <summary>
    /// A Property with a getter.
    /// </summary>
    Get,
    
    /// <summary>
    /// A Property with a setter.
    /// </summary>
    Set
};