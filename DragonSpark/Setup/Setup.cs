using DragonSpark.Runtime;

namespace DragonSpark.Setup
{
	public class Setup : CompositeCommand<ISetupParameter>, ISetup
	{
		public Collection<object> Items { get; } = new Collection<object>();

		protected override void OnExecute( ISetupParameter parameter )
		{
			parameter.AsRegistered<ISetup>( this );

			base.OnExecute( parameter );
		}
	}
}
