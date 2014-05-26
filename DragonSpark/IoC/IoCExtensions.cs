using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.IoC
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Io", Justification = "Using Resharper naming conventions." )]
	public static class IoCExtensions
	{
		readonly static IList<WeakReference> BuildCache = new List<WeakReference>();

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "BuildUp", Justification = "Following existing convention." )]
		public static bool BuildUpOnce( this object target )
		{
			var result = !BuildCache.Exists( target ); // && !Environment.IsInDesignMode;
			result.IsTrue( () => ServiceLocator.Current.TryGetInstance<IUnityContainer>().NotNull( x =>
			{
				x.BuildUp( target.GetType(), target );
				BuildCache.Add( new WeakReference(target) );
			}));
			return result;
		}

		/// <summary>
		/// The overridable implemenation of GetModelType.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible", Justification = "Strategy pattern." )]
		public static Func<Type, ConstructorInfo> SelectEligibleConstructorImplementation = DefaultSelectEligibleConstructor;

		/// <summary>
		/// Gets the preferred constructor for instantiation.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The constructor.</returns>
		public static ConstructorInfo SelectEligibleConstructor(this Type type)
		{
			return SelectEligibleConstructorImplementation(type);
		}

		private static ConstructorInfo DefaultSelectEligibleConstructor(Type type)
		{
			return (from c in type.GetConstructors()
			        orderby c.GetParameters().Length descending
			        select c).FirstOrDefault();
		}

		/// <summary>
		/// Determines whether the specified registration has a key.
		/// </summary>
		/// <param name="registration">The registration.</param>
		/// <returns>
		/// 	<c>true</c> if the specified registration has key; otherwise, <c>false</c>.
		/// </returns>
		public static bool HasName(this ComponentRegistrationBaseAttribute registration)
		{
			return !string.IsNullOrEmpty(registration.Name);
		}

		/// <summary>
		/// Determines whether the specified registration has a service.
		/// </summary>
		/// <param name="registration">The registration.</param>
		/// <returns>
		/// 	<c>true</c> if the specified registration has service; otherwise, <c>false</c>.
		/// </returns>
		public static bool HasService(this ComponentRegistrationBaseAttribute registration)
		{
			return registration.Service != null;
		}

		/// <summary>
		/// Determines whether the specified type is concrete.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		/// 	<c>true</c> if the specified type is concrete; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsConcrete(this Type type)
		{
			return !type.IsAbstract && !type.IsInterface;
		}

		/// <summary>
		/// Finds the interface that closes the open generic on the type if it exists.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="openGeneric">The open generic.</param>
		/// <returns>The interface type or null if not found.</returns>
		public static Type FindInterfaceThatCloses(this Type type, Type openGeneric)
		{
			if (!type.IsConcrete())
				return null;

			if (openGeneric.IsInterface)
			{
				foreach (var interfaceType in type.GetInterfaces())
				{
					if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == openGeneric)
					{
						return interfaceType;
					}
				}
			}
			else if (type.BaseType.IsGenericType &&
			         type.BaseType.GetGenericTypeDefinition() == openGeneric)
			{
				return type.BaseType;
			}

			return type.BaseType == typeof(object)
			       	? null
			       	: FindInterfaceThatCloses(type.BaseType, openGeneric);
		}  
	}
}