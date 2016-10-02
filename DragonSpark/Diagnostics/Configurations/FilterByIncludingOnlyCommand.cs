using Serilog.Configuration;

namespace DragonSpark.Diagnostics.Configurations
{
	public class FilterByIncludingOnlyCommand : FilterBySpecificationCommandBase
	{
		protected override void Configure( LoggerFilterConfiguration configuration ) => configuration.ByIncludingOnly( Specification.IsSatisfiedBy );
	}
}