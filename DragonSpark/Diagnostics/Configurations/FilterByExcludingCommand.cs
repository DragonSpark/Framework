using Serilog.Configuration;

namespace DragonSpark.Diagnostics.Configurations
{
	public class FilterByExcludingCommand : FilterBySpecificationCommandBase
	{
		protected override void Configure( LoggerFilterConfiguration configuration ) => configuration.ByExcluding( Specification.IsSatisfiedBy );
	}
}