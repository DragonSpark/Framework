using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity.Utility;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using DragonSpark.TypeSystem;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.ComponentModel
{
	public class BuildablePropertyCollectionFactory : FactoryBase<object, ICollection<PropertyInfo>>
	{
		public static BuildablePropertyCollectionFactory Instance { get; } = new BuildablePropertyCollectionFactory();

		protected override ICollection<PropertyInfo> CreateItem( object parameter )
		{
			var result = parameter.GetType().GetPropertiesHierarchical()
				.Where( x => Attributes.Get( x ).With( y => y.Has<DefaultValueAttribute>() || y.Has<DefaultValueBase>() ) )
				.Where( x => Equals( GetValue( parameter, x ), x.PropertyType.Adapt().GetDefaultValue() ) )
				.ToList();
			return result;
		}

		static object GetValue( object parameter, PropertyInfo x )
		{
			try
			{
				return x.GetValue( parameter );
			}
			catch ( TargetInvocationException )
			{
				return null;
			}
		}
	}
}