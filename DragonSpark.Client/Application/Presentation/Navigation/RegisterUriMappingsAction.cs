using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;
using System.Windows.Navigation;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Application.Presentation.Infrastructure;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Presentation.Navigation
{
	[ContentProperty( "Mappings" )]
	public class RegisterUriMappingsAction : RegionViewRegistrationAction
	{
		public Collection<UriMapping> Mappings
		{
			get { return mappings; }
		}	readonly Collection<UriMapping> mappings = new Collection<UriMapping>();

		protected internal override void Process( IUnityContainer container, IRegion region, Type viewType )
		{
			var behavior = region.GetBehavior<UriMappingContainerBehavior>();
			behavior.NotNull( x => EnsureMappings( viewType ).Apply( y => x.Mapper.UriMappings.FirstOrDefault( z => z.MappedUri == y.MappedUri && z.Uri == y.MappedUri ).Null( () => x.Mapper.UriMappings.Add( y ) ) ) );
		}

		IEnumerable<UriMapping> EnsureMappings( Type viewType )
		{
			var uri = new Uri( viewType.FullName, UriKind.Relative );
			Mappings.Apply( x => x.MappedUri = x.MappedUri ?? uri );
			return Mappings;
		}
	}
}