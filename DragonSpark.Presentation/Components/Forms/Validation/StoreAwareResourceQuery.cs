using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Presentation.Components.Forms.Validation;

sealed class StoreAwareResourceQuery : ReferenceStoring<string, ResourceQueryRecord?>, IResourceQuery
{
	public StoreAwareResourceQuery(IResourceQuery select)
		: base(Start.A.Selection<string>().By.Calling(string.Intern).Select(select)) {}
}