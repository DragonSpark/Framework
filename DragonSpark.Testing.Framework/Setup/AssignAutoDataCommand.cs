using DragonSpark.Runtime;
using DragonSpark.Runtime.Values;

namespace DragonSpark.Testing.Framework.Setup
{
	public class AssignAutoDataCommand : AssignValueCommand<AutoData>
	{
		public AssignAutoDataCommand() : this( new CurrentAutoDataContext() ) {}

		public AssignAutoDataCommand( IWritableValue<AutoData> value ) : base( value ) {}

		protected override void OnExecute( AutoData parameter )
		{
			base.OnExecute( parameter );
			parameter.Initialize();
		}
	}
}