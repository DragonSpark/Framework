using System.Linq;
using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.TypeSystem;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup.Registration
{
	public class RegisterFromMetadataCommand : Command<ConventionRegistrationProfile>
	{
		readonly IServiceRegistry registry;
		readonly IAttributeProvider provider;

		public RegisterFromMetadataCommand( [Required]IServiceRegistry registry ) : this( registry, AttributeProvider.Instance ) {}

		public RegisterFromMetadataCommand( [Required]IServiceRegistry registry, [Required]IAttributeProvider provider )
		{
			this.registry = registry;
			this.provider = provider;
		}

		protected override void OnExecute( ConventionRegistrationProfile parameter )
		{
			var tuples = provider.WhereDecorated<RegistrationBaseAttribute>( parameter.Candidates.AsTypeInfos() ).ToArray();
			tuples
				.Each( item => HostedValueLocator<IRegistration>.Instance.Create( item.Item2 ).Each( registration => { registration.Register( registry, item.Item2.AsType() ); } ) );
		}
	}
}