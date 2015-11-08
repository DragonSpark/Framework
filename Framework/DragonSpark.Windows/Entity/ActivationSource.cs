using System;
using System.Collections;
using System.Collections.ObjectModel;
using DragonSpark.Extensions;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Windows.Entity
{
	class ActivationSource : IActivationSource
	{
		public static ActivationSource Instance { get; } = new ActivationSource();

		readonly Collection<Type> watching = new Collection<Type>();

		public void Apply( object item )
		{
			var type = item.GetType();
			var canActivate = Activator.CanActivate( type );
			if ( canActivate && !watching.Contains( type ) )
			{
				using ( new Context( watching, type ) )
				{
					var instance = Activator.Create( type );
					if ( instance != item )
					{
						instance.MapInto( item, Mappings.OnlyProvidedValues() );
					}
				}
			}
		}

		class Context : IDisposable
		{
			readonly IList items;
			readonly Type item;

			public Context( IList items, Type item )
			{
				this.items = items;
				this.item = item;
				items.Add( item );
			}

			public void Dispose()
			{
				items.Remove( item );
			}
		}
	}
}