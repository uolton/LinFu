﻿using System;
using System.Linq;
using LinFu.AOP.Cecil.Interfaces;
using LinFu.AOP.Interfaces;
using Mono.Cecil;

namespace LinFu.AOP.Cecil.Extensions
{
    /// <summary>
    ///     An extension class that adds support for intercepting the 'new' operator with LinFu.AOP.
    /// </summary>
    public static class NewOperatorInterceptionExtensions
    {
        /// <summary>
        ///     Modifies a <paramref name="target" /> to support intercepting all calls to the 'new' operator.
        /// </summary>
        /// <param name="target">The assembly to be modified.</param>
        public static void InterceptAllNewInstances(this AssemblyDefinition target)
        {
            var typeFilter = GetTypeFilter();
            target.InterceptNewInstances(typeFilter);
        }

        /// <summary>
        ///     Modifies a <paramref name="target" /> to support intercepting all calls to the 'new' operator.
        /// </summary>
        /// <param name="target">The assembly to be modified.</param>
        public static void InterceptAllNewInstances(this TypeDefinition target)
        {
            var typeFilter = GetTypeFilter();
            target.InterceptNewInstances(typeFilter);
        }

        /// <summary>
        ///     Modifies a <paramref name="target" /> to support intercepting calls to the 'new' operator.
        /// </summary>
        /// <param name="target">The assembly to be modified.</param>
        /// <param name="typeFilter">The functor that determines which type instantiations should be intercepted.</param>
        /// <param name="methodFilter">The filter that determines which host methods will be modified</param>
        /// <remarks>
        ///     The type filter determines the concrete types that should be intercepted at runtime.
        ///     For example, the following functor code intercepts types named "Foo":
        ///     <code>
        ///     Func&lt;TypeReference, bool&gt; filter = 
        ///     concreteType => concreteType.Name == "Foo";
        /// </code>
        /// </remarks>
        public static void InterceptNewInstances(this TypeDefinition target, Func<TypeReference, bool> typeFilter,
            Func<MethodReference, bool> methodFilter)
        {
            Func<MethodReference, TypeReference, bool> constructorFilter =
                (constructor, declaringType) => methodFilter(constructor) && typeFilter(declaringType);

            Func<MethodReference, TypeReference, MethodReference, bool> filter =
                (ctor, declaringType, declaringMethod) =>
                    constructorFilter(ctor, declaringType) && methodFilter(declaringMethod);

            var redirector = new RedirectNewInstancesToActivator(filter);
            target.InterceptNewInstancesWith(redirector, methodFilter);
        }


        /// <summary>
        ///     Modifies a <paramref name="target" /> assembly to support intercepting calls to the 'new' operator.
        /// </summary>
        /// <param name="target">The assembly to be modified.</param>
        /// <param name="constructorFilter">The functor that determines which type instantiations should be intercepted.</param>
        /// <param name="methodFilter">The filter that determines which host methods will be modified</param>
        /// <remarks>
        ///     The type filter determines which concrete types and constructors should be intercepted at runtime.
        ///     For example, the following functor code intercepts types named "Foo":
        ///     <code>
        ///     Func&lt;MethodReference, TypeReference, bool&gt; filter = 
        ///     (constructor, concreteType, hostMethod) => concreteType.Name == "Foo";
        /// </code>
        /// </remarks>
        public static void InterceptNewInstances(this AssemblyDefinition target,
            Func<MethodReference, TypeReference, bool> constructorFilter,
            Func<MethodReference, bool> methodFilter)
        {
            Func<MethodReference, TypeReference, MethodReference, bool> filter =
                (ctor, declaringType, declaringMethod) =>
                    constructorFilter(ctor, declaringType) && methodFilter(declaringMethod);

            var redirector = new RedirectNewInstancesToActivator(filter);
            target.InterceptNewInstancesWith(redirector, methodFilter);
        }

        /// <summary>
        ///     Modifies a <paramref name="target" /> assembly to support intercepting calls to the 'new' operator.
        /// </summary>
        /// <param name="target">The assembly to be modified.</param>
        /// <param name="typeFilter">The functor that determines which type instantiations should be intercepted.</param>
        /// <remarks>
        ///     The type filter determines the concrete types that should be intercepted at runtime.
        ///     For example, the following functor code intercepts types named "Foo":
        ///     <code>
        ///     Func&lt;TypeReference, bool&gt; filter = 
        ///     concreteType => concreteType.Name == "Foo";
        /// </code>
        /// </remarks>
        public static void InterceptNewInstances(this AssemblyDefinition target,
            Func<TypeReference, bool> typeFilter)
        {
            target.InterceptNewInstances(typeFilter, m => true);
        }

        /// <summary>
        ///     Modifies a <paramref name="target" /> assembly to support intercepting calls to the 'new' operator.
        /// </summary>
        /// <param name="target">The assembly to be modified.</param>
        /// <param name="typeFilter">The functor that determines which type instantiations should be intercepted.</param>
        /// <remarks>
        ///     The type filter determines the concrete types that should be intercepted at runtime.
        ///     For example, the following functor code intercepts types named "Foo":
        ///     <code>
        ///     Func&lt;TypeReference, bool&gt; filter = 
        ///     concreteType => concreteType.Name == "Foo";
        /// </code>
        /// </remarks>
        public static void InterceptNewInstances(this TypeDefinition target, Func<TypeReference, bool> typeFilter)
        {
            target.InterceptNewInstances(typeFilter, m => true);
        }

        /// <summary>
        ///     Modifies the <paramref name="target" /> to support intercepting calls to the 'new' operator.
        /// </summary>
        /// <param name="target">The item to be modified.</param>
        /// <param name="methodFilter">The filter that determines which host methods will be modified</param>
        /// <param name="typeFilter">The filter that determines which types will be modified.</param>
        /// <remarks>
        ///     The type filter determines which concrete types and constructors should be intercepted at runtime.
        ///     For example, the following functor code intercepts types named "Foo":
        ///     <code>
        ///     Func&lt;MethodReference, TypeReference, bool&gt; filter = 
        ///     (constructor, concreteType, hostMethod) => concreteType.Name == "Foo";
        /// </code>
        /// </remarks>
        public static void InterceptNewInstances(this AssemblyDefinition target,
            Func<TypeReference, bool> typeFilter,
            Func<MethodReference, bool> methodFilter)
        {
            Func<MethodReference, TypeReference, bool> constructorFilter =
                (constructor, declaringType) => methodFilter(constructor) && typeFilter(declaringType);

            Func<MethodReference, TypeReference, MethodReference, bool> filter =
                (ctor, declaringType, declaringMethod) =>
                    constructorFilter(ctor, declaringType) && methodFilter(declaringMethod);

            var redirector = new RedirectNewInstancesToActivator(filter);
            target.InterceptNewInstancesWith(redirector, methodFilter);
        }


        /// <summary>
        ///     Modifies the <paramref name="target" /> to support intercepting calls to the 'new' operator.
        /// </summary>
        /// <param name="target">The item to be modified.</param>
        /// <param name="constructorFilter">The functor that determines which type instantiations should be intercepted.</param>
        /// <param name="methodFilter">The filter that determines which host methods will be modified</param>
        /// <remarks>
        ///     The type filter determines which concrete types and constructors should be intercepted at runtime.
        ///     For example, the following functor code intercepts types named "Foo":
        ///     <code>
        ///     Func&lt;MethodReference, TypeReference, bool&gt; filter = 
        ///     (constructor, concreteType, hostMethod) => concreteType.Name == "Foo";
        /// </code>
        /// </remarks>
        public static void InterceptNewInstances(this TypeDefinition target,
            Func<MethodReference, TypeReference, bool> constructorFilter,
            Func<MethodReference, bool> methodFilter)
        {
            Func<MethodReference, TypeReference, MethodReference, bool> filter =
                (ctor, declaringType, declaringMethod) =>
                    constructorFilter(ctor, declaringType) && methodFilter(declaringMethod);

            var redirector = new RedirectNewInstancesToActivator(filter);
            target.InterceptNewInstancesWith(redirector, methodFilter);
        }

        /// <summary>
        ///     Modifies the <paramref name="target" /> to support intercepting calls to the 'new' operator.
        /// </summary>
        /// <param name="target">The item to be modified.</param>
        /// <param name="newInstanceFilter">The filter that will determine which constructor calls should be intercepted.</param>
        /// <param name="methodFilter">
        ///     The filter that will determine which host methods should be modified to support new instance
        ///     interception.
        /// </param>
        public static void InterceptNewInstances(this AssemblyDefinition target,
            INewInstanceFilter newInstanceFilter, IMethodFilter methodFilter)
        {
            var redirector = new RedirectNewInstancesToActivator(newInstanceFilter);
            target.InterceptNewInstancesWith(redirector, methodFilter.ShouldWeave);
        }

        /// <summary>
        ///     Modifies the <paramref name="target" /> to support intercepting calls to the 'new' operator.
        /// </summary>
        /// <param name="target">The item to be modified.</param>
        /// <param name="newInstanceFilter">The filter that will determine which constructor calls should be intercepted.</param>
        /// <param name="methodFilter">
        ///     The filter that will determine which host methods should be modified to support new instance
        ///     interception.
        /// </param>
        public static void InterceptNewInstances(this TypeDefinition target, INewInstanceFilter newInstanceFilter,
            IMethodFilter methodFilter)
        {
            var redirector = new RedirectNewInstancesToActivator(newInstanceFilter);
            target.InterceptNewInstancesWith(redirector, methodFilter.ShouldWeave);
        }

        /// <summary>
        ///     Modifies the methods in the given <paramref name="target" /> using the custom <see cref="INewObjectWeaver" />
        ///     instance.
        /// </summary>
        /// <param name="target">The host that contains the methods that will be modified.</param>
        /// <param name="weaver">
        ///     The custom <see cref="INewObjectWeaver" /> that will replace all calls to the new operator with
        ///     the custom code emitted by the given weaver.
        /// </param>
        /// <param name="filter">The method filter that will determine which methods should be modified.</param>
        public static void InterceptNewInstancesWith(this AssemblyDefinition target, INewObjectWeaver weaver,
            Func<MethodReference, bool> filter)
        {
            IMethodWeaver interceptNewCalls = new InterceptNewCalls(weaver);
            var module = target.MainModule;
            var targetMethods = module.Types.SelectMany(t => t.Methods).Where(m => filter(m)).ToArray();
            foreach (var targetMethod in targetMethods)
            {
                interceptNewCalls.Weave(targetMethod);
            }
        }

        /// <summary>
        ///     Modifies the methods in the given <paramref name="target" /> using the custom <see cref="INewObjectWeaver" />
        ///     instance.
        /// </summary>
        /// <param name="target">The host that contains the methods that will be modified.</param>
        /// <param name="weaver">
        ///     The custom <see cref="INewObjectWeaver" /> that will replace all calls to the new operator with
        ///     the custom code emitted by the given weaver.
        /// </param>
        /// <param name="filter">The method filter that will determine which methods should be modified.</param>
        public static void InterceptNewInstancesWith(this TypeDefinition target, INewObjectWeaver weaver,
            Func<MethodReference, bool> filter)
        {
            var interceptNewCalls = new InterceptNewCalls(weaver);
            var targetMethods = target.Methods.Where(m => filter(m)).ToArray();

            foreach (var targetMethod in targetMethods)
            {
                interceptNewCalls.Weave(targetMethod);
            }
        }

        private static Func<TypeReference, bool> GetTypeFilter()
        {
            return type =>
            {
                var result = !type.IsValueType;

                var module = type.Module;
                var moduleName = module.Name;

                if (moduleName.StartsWith("LinFu.AOP"))
                    return false;

                return result;
            };
        }
    }
}