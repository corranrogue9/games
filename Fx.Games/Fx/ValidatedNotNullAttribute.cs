namespace Fx
{
    using System;

    /// <summary>
    /// Used to notify Code Analysis that the attributed parameter has been validated to not be null once the method returns
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    internal sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}