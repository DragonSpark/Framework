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

		readonly IAttributeProvider provider;

		public BuildablePropertyCollectionFactory() : this( AttributeProvider.Instance ) {}

		public BuildablePropertyCollectionFactory( [Required]IAttributeProvider provider )
		{
			this.provider = provider;
		}

		protected override ICollection<PropertyInfo> CreateItem( object parameter )
		{
			var result = parameter.GetType().GetPropertiesHierarchical()
				.Where( x => provider.IsDecoratedWith<DefaultValueAttribute>( x ) || provider.IsDecoratedWith<DefaultValueBase>( x ) )
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