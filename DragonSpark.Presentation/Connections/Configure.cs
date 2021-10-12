using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Presentation.Connections
{
	sealed class Configure : IAlteration<BuildHostContext>
	{
		public static Configure Default { get; } = new Configure();

		Configure() {}

		public BuildHostContext Get(BuildHostContext parameter) => parameter.Configure(Registrations.Default);
	}
}