using System;
using System.Security.Principal;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.IoC;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace DragonSpark.Application.Presentation.Entity.Security
{
    [Singleton( typeof(IProfileValidationManager) )]
    public class ProfileValidationManager : ViewObject, IProfileValidationManager
    {
        readonly IProfileValidator validator;
        readonly IPrincipal principal;
        readonly string dialogTitle;

        public ProfileValidationManager( IProfileValidator validator, IPrincipal principal, string dialogTitle = "Complete Your Profile" )
        {
            this.validator = validator;
            this.principal = principal;
            this.dialogTitle = dialogTitle;
        }

        public void Validate( Action<ProfileValidationConfirmation> callback )
        {
            var valid = validator.IsValid( principal );
            var confirmation = new ProfileValidationConfirmation( principal, dialogTitle );
            if ( valid )
            {
                callback( confirmation );
            }
            else
            {
                displayInterface.Raise( confirmation, callback );
            }
        }

        public IInteractionRequest DisplayInterface
        {
            get { return displayInterface; }
        }	readonly InteractionRequest<ProfileValidationConfirmation> displayInterface = new InteractionRequest<ProfileValidationConfirmation>();
    }
}