using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.Forms.Validation;

sealed class StoreAwareResourceQuery : ReferenceStoring<string, ResourceQueryRecord?>, IResourceQuery
{
	public StoreAwareResourceQuery(ResourceQuery select)
		: base(Start.A.Selection<string>().By.Calling(string.Intern).Select(select)) {}
}