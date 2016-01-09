using System;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using DragonSpark.TypeSystem;

namespace DragonSpark.ComponentModel
{
	public class DefaultPropertyValueFactory : FactoryBase<DefaultValueParameter, object>
	{
		public static DefaultPropertyValueFactory Instance { get; } = new DefaultPropertyValueFactory();

		readonly IAttributeProvider provider;
		readonly Func<MemberInfo, IDefaultValueProvider[]> factory;

		public DefaultPropertyValueFactory() : this( AttributeProvider.Instance, HostedValueLocator<IDefaultValueProvider>.Instance.Create )
		{}

		public DefaultPropertyValueFactory( [Required]IAttributeProvider provider, [Required]Func<MemberInfo, IDefaultValueProvider[]> factory )
		{
			this.provider = provider;
			this.factory = factory;
		}

		protected override object CreateItem( DefaultValueParameter parameter )
		{
			var result = factory( parameter.Metadata ).Select( provider => provider.GetValue( parameter ) ).NotNull().FirstOrDefault()
						 ??
						 provider.FromMetadata<DefaultValueAttribute, object>( parameter.Metadata, attribute => attribute.Value );
			return result;
		}
	}

	public class DefaultValueParameter
	{
		public DefaultValueParameter( [Required]object instance, [Required]PropertyInfo metadata )
		{
			Instance = instance;
			Metadata = metadata;
		}

		public object Instance { get; }

		public PropertyInfo Metadata { get; }

		public DefaultValueParameter Assign( object value )
		{
			Metadata.SetValue( Instance, value );
			return this;
		}

		/*protected bool Equals( DefaultValueParameter other )
		{
			return Equals( Instance, other.Instance ) && Equals( Metadata, other.Metadata );
		}

		public override bool Equals( object obj )
		{
			return !ReferenceEquals( null, obj ) && ( ReferenceEquals( this, obj ) || obj.GetType() == this.GetType() && Equals( (DefaultValueParameter)obj ) );
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var result = Instance.GetHashCode() * 397 ^ Metadata.GetHashCode();
				return result;
			}
		}*/
	}

}