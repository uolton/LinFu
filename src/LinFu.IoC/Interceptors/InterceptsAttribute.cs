﻿using System;

namespace LinFu.IoC.Interceptors
{
    /// <summary>
    ///     The attribute class used to indentify interceptor classes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class InterceptsAttribute : Attribute
    {
        /// <summary>
        ///     Initializes the class with the given <paramref name="targetType" />.
        /// </summary>
        /// <param name="targetType">The target type that will be intercepted.</param>
        public InterceptsAttribute(Type targetType)
        {
            TargetType = targetType;
        }

        /// <summary>
        ///     Initializes the class with the given <paramref name="targetType" /> and <paramref name="serviceName" />.
        /// </summary>
        /// <param name="serviceName">The name of service that will be intercepted.</param>
        /// <param name="targetType">The target type that will be intercepted.</param>
        public InterceptsAttribute(string serviceName, Type targetType) : this(targetType)
        {
            ServiceName = serviceName;
        }

        /// <summary>
        ///     Gets the value indicating the name of the service to intercept.
        /// </summary>
        public string ServiceName { get; }

        /// <summary>
        ///     Gets the value indicating the target type that will be intercepted.
        /// </summary>
        public Type TargetType { get; }
    }
}