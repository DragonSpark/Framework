using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System.Threading.Tasks;

namespace DragonSpark.Services.Compose
{


	public sealed class Build
	{
		public static Build A { get; } = new Build();

		Build() {}

		public ProgramLocationContext Program { get; } = ProgramLocationContext.Instance;

		public sealed class ProgramLocationContext
		{
			public static ProgramLocationContext Instance { get; } = new ProgramLocationContext();

			ProgramLocationContext() {}

			public ProgramLocationByContext By { get; } = ProgramLocationByContext.Default;
		}
	}

	public sealed class ProgramLocationByContext
	{
		public static ProgramLocationByContext Default { get; } = new ProgramLocationByContext();

		ProgramLocationByContext() {}

		public ProgramContext Environment() => Activating<LocatedProgram>();

		public ProgramContext Environment(IProgram @default) => Using(new LocatedProgram(@default));

		public ProgramContext Using(IProgram program) => new ProgramContext(program);

		public ProgramContext Activating<T>() where T : class, IProgram => Using(Start.An.Instance<T>());
	}

	public sealed class ProgramContext
	{
		readonly IProgram _program;

		public ProgramContext(IProgram program) => _program = program;

		public ISelect<Array<string>, Task> ConfiguredBy<T>() where T : class, IConfigurator
			=> HostBuilder<T>.Default.Select(_program);
	}
}
