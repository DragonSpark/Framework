using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using System.Linq;
using System.Windows.Markup;
using DragonSpark.Runtime;

namespace DragonSpark.Setup.Commands
{
	[ContentProperty( nameof(Instances) )]
	public class ConfigureUnityCommand : UnityCommand
	{
		public System.Collections.ObjectModel.Collection<UnityContainerExtension> Extensions { get; } = new System.Collections.ObjectModel.Collection<UnityContainerExtension>();

		public CommandCollection<UnityType> Types { get; } = new CommandCollection<UnityType>();

		public UnityInstanceCollection Instances { get; } = new UnityInstanceCollection();

		/*public CommandCollection PreConfigurations { get; } = new CommandCollection();

		public CommandCollection Configurations { get; } = new CommandCollection();

		public CommandCollection PostConfigurations { get; } = new CommandCollection();*/

		protected override void OnExecute( ISetupParameter parameter )
		{
			Extensions.Each( x => Container.AddExtension( x ) );

			var commands = Instances.Cast<UnityCommand>().Concat( Types ).ToArray();
			commands.ExecuteWith<UnityCommand>( parameter );
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
