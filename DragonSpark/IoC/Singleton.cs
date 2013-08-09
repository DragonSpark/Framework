using System;

namespace DragonSpark.IoC
{
	/// <summary>
	/// Represents a Singleton registration.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
	public sealed class SingletonAttribute : ComponentRegistrationBaseAttribute
	{
		/// <summary>
		/// Gets or sets the implementation.
		/// </summary>
		/// <value>The implementation.</value>
		public Type Implementation { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SingletonAttribute"/> class.
		/// </summary>
		public SingletonAttribute() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="SingletonAttribute"/> class.
		/// </summary>
		/// <param name="service">The type key.</param>
		public SingletonAttribute(Type service)
		{
			Service = service;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SingletonAttribute"/> class.
		/// </summary>
		/// <param name="name">The key.</param>
		/// <param name="service">The service.</param>
		public SingletonAttribute(string name, Type service)
		{
			Name = name;
			Service = service;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SingletonAttribute"/> class.
		/// </summary>
		/// <param name="name">The key.</param>
		public SingletonAttribute(string name)
		{
			Name = name;
		}

		/// <summary>
		/// Gets the component info.
		/// </summary>
		/// <param name="decoratedType">Type of the decorated.</param>
		/// <returns></returns>
		public override IComponentRegistration GetComponentInfo(Type decoratedType)
		{
			Implementation = decoratedType;
			if (Service == null) Service = decoratedType;
			return this;
		}
	}
}