using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using PostSharp.Patterns.Contracts;
using System.Linq;

namespace DragonSpark.Setup.Registration
{
	public class RegisterFromMetadataCommand : Command<ConventionRegistrationProfile>
	{
		readonly IServiceRegistry registry;

		public RegisterFromMetadataCommand( [Required]IServiceRegistry registry )
		{
			this.registry = registry;
		}

		protected override void OnExecute( ConventionRegistrationProfile parameter )
		{
			var tuples = parameter.Candidates.AsTypeInfos().WhereDecorated<RegistrationBaseAttribute>().ToArray();
			tuples
				.Each( item => HostedValueLocator<IRegistration>.Instance.Create( item.Item2 ).Each( registration => registration.Register( registry ) ) );
		}
	}
}