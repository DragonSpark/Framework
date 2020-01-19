using DragonSpark.Compose;

namespace DragonSpark.Application
{
	public sealed class ProgramLocationByContext
	{
		public static ProgramLocationByContext Default { get; } = new ProgramLocationByContext();

		ProgramLocationByContext() {}

		public ProgramContext Environment() => Activating<LocatedProgram>();

		public ProgramContext Environment(IProgram @default) => Using(new LocatedProgram(@default));

		public ProgramContext Using(IProgram program) => new ProgramContext(program);

		public ProgramContext Activating<T>() where T : class, IProgram => Using(Start.An.Instance<T>());
	}
}