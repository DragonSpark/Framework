using System;
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Entity.Security
{
    public class LoadAndValidateUserOperation : LoadUserOperation
    {
        readonly IProfileValidationManager manager;

        public LoadAndValidateUserOperation( AuthenticationService authenticationService, IProfileValidationManager manager ) : base( authenticationService )
        {
            this.manager = manager;
        }

        protected override void Continue()
        {
            manager.Validate( x =>
            {
                Exception = Exception ?? x.Exception.Transform( y => new InvalidOperationException( "Profile is not valid, dawg.", y ) );
                base.Continue();
            } );
        }

        /*[DefaultPropertyValue( "Validating Account Information" )]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}*/
    }
}