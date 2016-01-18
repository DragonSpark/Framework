using System;
using System.Reflection;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.Extensions;
using DragonSpark.Setup.Registration;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Activation.IoC
{
	public class LifetimeManagerFactory<T> : LifetimeManagerFactory where T : LifetimeManager
	{
		public LifetimeManagerFactory( IUnityContainer container ) : base( container ) {}

		protected override Type DetermineType( Type parameter ) => base.DetermineType( parameter ) ?? typeof(T);
	}

	[Persistent]
	public class LifetimeManagerFactory : FactoryBase<Type, LifetimeManager>
	{
		readonly IUnityContainer container;

		public LifetimeManagerFactory( [Required]IUnityContainer container )
		{
			this.container = container;
		}

		[Freeze]
		protected override LifetimeManager CreateItem( Type parameter )
		{
			var type = DetermineType( parameter );
			var result = type.With( container.Resolve<LifetimeManager> );
			return result;
		}

		protected virtual Type DetermineType( Type parameter ) => parameter.GetTypeInfo().GetCustomAttribute<LifetimeManagerAttribute>().AsTo<LifetimeManagerAttribute, Type>( x => x.LifetimeManagerType );
	}
}