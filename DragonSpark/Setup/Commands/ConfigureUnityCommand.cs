using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Setup.Commands
{
	[ContentProperty( nameof(Instances) )]
	public class ConfigureUnityCommand : UnityCommand
	{
		public Collection<UnityContainerExtension> Extensions { get; } = new Collection<UnityContainerExtension>();

		public CommandCollection<UnityType> Types { get; } = new CommandCollection<UnityType>();

		public UnityInstanceCollection Instances { get; } = new UnityInstanceCollection();

		public CommandCollection PreConfigurations { get; } = new CommandCollection();

		public CommandCollection Configurations { get; } = new CommandCollection();

		public CommandCollection PostConfigurations { get; } = new CommandCollection();

		protected override void Execute( SetupContext context )
		{
			Extensions.Each( x => Container.AddExtension( x ) );

			var commands = PreConfigurations.Concat( Instances ).Concat( Configurations ).Concat( Types ).Concat( PostConfigurations ).ToArray();
			commands.Where( command => command.CanExecute( context ) ).Each( item => item.Execute( context ) );
		}
	}

	// [ConvertItem]
	public class UnityInstanceCollection : CommandCollection<UnityInstance>
	{
		protected override UnityInstance OnAdd( object item )
		{
			var result = base.OnAdd( item ) ?? new UnityInstance { Instance = item, RegistrationType = item.GetType() };
			return result;
		}
	}

	/*[Serializable]
	public class AddAspect : MethodLevelAspect, IAspectProvider
	{
		[SelfPointcut]
		[OnMethodSuccessAdvice]
		public void OnEntry( MethodExecutionArgs args )
		{}

		public IEnumerable<AspectInstance> ProvideAspects( object targetElement )
		{
			throw new NotImplementedException();
		}
	}*/


	/*[PSerializable]
	public class ConvertItem : InstanceLevelAspect
	{
		[OnMethodEntryAdvice, MethodPointcut( nameof( Select ) )]
		public void OnEntry( MethodExecutionArgs args )
		{
			
		}

		IEnumerable<MethodInfo> Select( Type type )
		{
			yield return typeof(IList).GetRuntimeMethod( nameof(IList.Add), new [] { typeof(object) } );
		}
	}*/
}
