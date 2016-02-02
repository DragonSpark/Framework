using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Setup.Commands;
using PostSharp.Patterns.Contracts;
using System.Linq;

namespace DragonSpark.Setup.Registration
{
	public class DelegatedRegisterFromMetadataCommand : DelegatedCommand<RegisterFromMetadataCommand, ConventionRegistrationProfile> {}

	public class RegisterFromMetadataCommand : Command<ConventionRegistrationProfile>
	{
		readonly IServiceRegistry registry;

		public RegisterFromMetadataCommand( [Required]PersistentServiceRegistry registry )
		{
			this.registry = registry;
		}

		protected override void OnExecute( ConventionRegistrationProfile parameter ) => 
			parameter.Candidates.AsTypeInfos()
				.WhereDecorated<RegistrationBaseAttribute>()
				.Select( item => item.Item2 )
				.SelectMany( HostedValueLocator<IRegistration>.Instance.Create )
				.Each( registration => registration.Register( registry ) );
	}
}