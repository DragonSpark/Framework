using DragonSpark.Model.Results;

namespace DragonSpark.Application;

public sealed class ProgramContext : Instance<IProgram>
{
	public ProgramContext(IProgram program) : base(program) {}
}