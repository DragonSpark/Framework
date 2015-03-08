using System.IO.IsolatedStorage;
using DragonSpark.Application.Presentation.Commands;
using DragonSpark.Application.Presentation.Extensions;

namespace DragonSpark.Application.Presentation.Infrastructure
{
    public class SelectProfileCommand : CommandBase<ViewProfile>
    {
        readonly IViewProfileService service;
        readonly IsolatedStorageSettings settings;

        public SelectProfileCommand( IViewProfileService service, IsolatedStorageSettings settings )
        {
            this.service = service;
            this.settings = settings;
        }

        protected override bool CanExecute( ViewProfile parameter )
        {
            return base.CanExecute( parameter ) && service.SelectedProfile != parameter;
        }

        protected override void Execute( ViewProfile parameter )
        {
            settings.Set<IViewProfileService,string>( x => x.SelectedProfile.Identifier, parameter.Identifier );
            service.Select( parameter );
        }
    }
}