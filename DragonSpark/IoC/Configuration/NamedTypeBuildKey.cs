using DragonSpark.Configuration;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using Microsoft.Practices.Unity;
using System;
using System.Windows.Markup;

namespace DragonSpark.IoC.Configuration
{
	[MarkupExtensionReturnType( typeof(NamedTypeBuildKey) )]
	public class NamedTypeBuildKeyExtension : MarkupExtension
	{
		readonly Type type;
		readonly string name;

		public NamedTypeBuildKeyExtension( Type type ) : this( type, null )
		{}

		public NamedTypeBuildKeyExtension( Type type, string name )
		{
			this.type = type;
			this.name = name;
		}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = new NamedTypeBuildKey{ BuildType = type, BuildName = name };
			return result;
		}
	}

	public class NamedTypeBuildKey : IInstanceSource<Microsoft.Practices.ObjectBuilder2.NamedTypeBuildKey>, IFactory
	{
		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type BuildType { get; set; }

		public string BuildName { get; set; }

		public Microsoft.Practices.ObjectBuilder2.NamedTypeBuildKey Instance
		{
			get
			{
				var result = new Microsoft.Practices.ObjectBuilder2.NamedTypeBuildKey( BuildType, BuildName );
				return result;
			}
		}

		public object Create( Type resultType, object source = null )
		{
			var result = source.AsTo<IUnityContainer, object>( x => this.Create( x, resultType ) );
			return result;
		}
	}
}