using DragonSpark.Windows.Runtime;

namespace DragonSpark.Testing.Framework.Runtime
{
	public sealed class Identification : TaskLocalStore<Identifier>
	{
		public static Identification Default { get; } = new Identification();
		Identification() {}

		public override Identifier Get()
		{
			var current = base.Get();
			var result = current == default(Identifier) ? Create() : current;
			return result;
		}

		Identifier Create()
		{
			var result = Identifier.Current();
			Assign( result );
			return result;
		}
	}
}