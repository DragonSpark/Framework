using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public class DefaultPropertyValueFactory : FactoryBase<DefaultValueParameter, object>
	{
		public static DefaultPropertyValueFactory Instance { get; } = new DefaultPropertyValueFactory();

		readonly IFactory<MemberInfo, IDefaultValueProvider[]> factory;

		public DefaultPropertyValueFactory() : this( HostedValueLocator<IDefaultValueProvider>.Instance )
		{}

		public DefaultPropertyValueFactory( [Required]IFactory<MemberInfo, IDefaultValueProvider[]> factory )
		{
			this.factory = factory;
		}

		protected override object CreateItem( DefaultValueParameter parameter )
		{
			var result = factory.Create( parameter.Metadata ).Select( provider => provider.GetValue( parameter ) ).NotNull().FirstOrDefault()
						 ??
						 parameter.Metadata.FromMetadata<DefaultValueAttribute, object>( attribute => attribute.Value );
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