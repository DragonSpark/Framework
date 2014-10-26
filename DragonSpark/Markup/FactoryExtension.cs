using System;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.Markup
{
	public class FactoryExtension : LocateExtension
	{
		public FactoryExtension()
		{}

		public FactoryExtension( Type type ) : base( type) 
		{}

		public FactoryExtension( Type type, object parameter ) : base( type )
		{
			Parameter = parameter;
		}

		public FactoryExtension( Type type, object parameter, string buildName ) : base( type, buildName )
		{
			Parameter = parameter;
		}

		protected override object Create( Type type )
		{
			var instance = Instance ?? base.Create( Type );
			var factory = instance.AsTo<IFactory,object>( x => x.Create( type, Parameter ) );
			return factory;
		}

		public IFactory Instance { get; set; }

		public object Parameter { get; set; }
	}
}