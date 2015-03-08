using System.Collections.Generic;
using System.Collections.ObjectModel;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.IoC;

namespace DragonSpark.Application.Presentation.Infrastructure
{
    [Singleton( typeof(IViewProfileService), Priority = Priority.Lowest )]
    public class ViewProfileService : ViewObject, IViewProfileService
    {
        readonly ViewCollection<ViewProfile> source = new ViewCollection<ViewProfile>();
        readonly ReadOnlyObservableCollection<ViewProfile> items;

        public ViewProfileService()
        {
            items = new ReadOnlyObservableCollection<ViewProfile>( source );
        }

        public ViewProfile SelectedProfile
        {
            get { return selectedProfile; }
            private set { SetProperty( ref selectedProfile, value, () => SelectedProfile ); }
        }	ViewProfile selectedProfile;

        public void Select( ViewProfile profile )
        {
            SelectedProfile = profile;
        }

        public void Register( ViewProfile profile )
        {
            source.Add( profile );
        }

        public IEnumerable<ViewProfile> Profiles
        {
            get { return items; }
        }
    }
}