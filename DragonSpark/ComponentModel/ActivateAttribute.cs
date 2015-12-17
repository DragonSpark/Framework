using DragonSpark.Activation;
using DragonSpark.Aspects;
using System;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public class ActivateAttribute : DefaultValueBase
	{
		public ActivateAttribute() : this( null )
		{}

		public ActivateAttribute( string name ) : this( null, name )
		{}

		public ActivateAttribute( Type activatedType, string name = null ) : this( typeof(ActivatedValueProvider), activatedType, name )
		{}

		public ActivateAttribute( [OfType( typeof(ActivatedValueProvider) )]Type surrogateType, Type activatedType, string name = null ) : base( surrogateType )
		{
			ActivatedType = activatedType;
			Name = name;
		}

		public Type ActivatedType { get; }
		public string Name { get; }
	}

	public class ActivatedValueProvider : IDefaultValueProvider
	{
		public ActivatedValueProvider( Type activatedType, string name ) : this( Activation.Activator.Current, activatedType, name )
		{}

		public ActivatedValueProvider(IActivator activator, Type activatedType, string name )
		{
			Activator = activator;
			ActivatedType = activatedType;
			Name = name;
		}

		protected IActivator Activator { get; }
		protected Type ActivatedType { get; }
		protected string Name { get; }

		public object GetValue( DefaultValueParameter parameter )
		{
			var type = ActivatedType ?? DetermineType( parameter.Metadata );
			var result = Activate( parameter, type );
			return result;
		}

		protected virtual object Activate( DefaultValueParameter parameter, Type qualified )
		{
			var result = Activator.Activate<object>( qualified, Name );
			return result;
		}

		protected virtual Type DetermineType( PropertyInfo propertyInfo )
		{
			return propertyInfo.PropertyType;
		}
	}
}