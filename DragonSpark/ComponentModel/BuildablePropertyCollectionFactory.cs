using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using TypeServices = DragonSpark.TypeSystem.Type;

namespace DragonSpark.ComponentModel
{
	public class BuildablePropertyCollectionFactory : FactoryBase<object, ICollection<PropertyInfo>>
	{
		public static BuildablePropertyCollectionFactory Instance { get; } = new BuildablePropertyCollectionFactory();

		protected override ICollection<PropertyInfo> CreateItem( object parameter )
		{
			var result = TypeServices.From( parameter ).GetTypeInfo().DeclaredProperties
				.Where( x => Attributes.Get( x ).With( y => y.Has<DefaultValueAttribute>() || y.Has<DefaultValueBase>() ) )
				// .Where( x => Equals( GetValue( parameter, x ), x.PropertyType.Adapt().GetDefaultValue() ) )
				.ToList();
			return result;
		}

		/*static object GetValue( object parameter, PropertyInfo x )
		{
			try
			{
				return x.GetValue( parameter );
			}
			catch ( TargetInvocationException )
			{
				return null;
			}
		}*/
	}
}