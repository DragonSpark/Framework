using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities {
	sealed class Initializer : IInitializer
	{
		public static Initializer Default { get; } = new Initializer();

		Initializer() {}

		public void Execute(ModelBuilder parameter) {}
	}
}