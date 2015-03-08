using System;
using System.Security.Principal;
using DragonSpark.Application.Presentation.Infrastructure;

namespace DragonSpark.Application.Presentation.Entity.Security
{
    public class ProfileValidationConfirmation : BasicNotification
    {
        readonly IPrincipal principal;

        public ProfileValidationConfirmation( IPrincipal principal, string title ) : base( title )
        {
            this.principal = principal;
        }

        public Exception Exception { get; set; }

        public IPrincipal Principal
        {
            get { return principal; }
        }
    }
}