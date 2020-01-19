namespace DragonSpark.Application {
	sealed class LocatedProgram : Program
	{
		public static LocatedProgram Default { get; } = new LocatedProgram();

		LocatedProgram() : this(DefaultProgram.Default) {}

		public LocatedProgram(IProgram @default) : base(/*Start.A.Result.Of.Type<IProgram>()
		                                                     .By.Location.Or.Default(@default)
		                                                     .Assume()*/@default) {}
	}
}