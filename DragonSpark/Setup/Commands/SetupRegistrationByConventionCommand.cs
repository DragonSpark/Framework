using DragonSpark.ComponentModel;
using DragonSpark.Setup.Registration;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup.Commands
{
	public abstract class SetupRegistrationByConventionCommand<TProvider, TService> : UnityRegistrationCommand 
		where TProvider : IConventionRegistrationProfileProvider
		where TService : IConventionRegistrationService
	{
		protected override void OnExecute( ISetupParameter parameter )
		{
			var profile = Provider.Retrieve();
			Service.Register( profile );
		}
		
		[Required, Activate]
		public TProvider Provider { [return: Required]get; set; }

		[Required, Activate]
		public TService Service { [return: Required]get; set; }
	}
}