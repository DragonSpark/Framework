using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Presentation
{
	public sealed class Configure : IAlteration<BuildHostContext>
	{
		public static Configure Default { get; } = new Configure();

		Configure() {}

		public BuildHostContext Get(BuildHostContext parameter) => parameter.To(Application.Configure.Default)
		                                                                    .Configure(DefaultRegistrations.Default);
	}
}