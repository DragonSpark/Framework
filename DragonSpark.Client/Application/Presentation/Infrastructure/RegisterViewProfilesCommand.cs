using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows.Markup;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Presentation.Infrastructure
{
    [ContentProperty( "Profiles" )]
    public class RegisterViewProfilesCommand : IContainerConfigurationCommand
    {
        public void Configure( IUnityContainer container )
        {
            var service = container.Resolve<IViewProfileService>();
            Profiles.Apply( service.Register );
			
            service.SelectedProfile.Null( () =>
            {
                var settings = container.Resolve<IsolatedStorageSettings>();
                var item = Profiles.FirstOrDefault( x => settings.Get<IViewProfileService,string>( y => y.SelectedProfile.Identifier ).Transform( y => y == x.Identifier, () => x.IsSelected ) );
                item.NotNull( service.Select );
            } );
        }

        public Collection<ViewProfile> Profiles
        {
            get { return profiles; }
        }	readonly Collection<ViewProfile> profiles = new Collection<ViewProfile>();
    }
}