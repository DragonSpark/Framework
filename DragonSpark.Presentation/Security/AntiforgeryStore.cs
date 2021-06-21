using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Antiforgery;

namespace DragonSpark.Presentation.Security
{
	public sealed class AntiforgeryStore : Variable<AntiforgeryTokenSet> {}
}