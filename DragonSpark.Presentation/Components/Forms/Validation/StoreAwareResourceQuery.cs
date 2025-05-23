using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Presentation.Components.Forms.Validation;

sealed class StoreAwareResourceQuery : ReferenceStoring<Stop<string>, ResourceQueryRecord?>, IResourceQuery
{
	public StoreAwareResourceQuery(IResourceQuery select)
		: base(Start.A.Selection<Stop<string>>().By.Calling(x => x.Subject).Select(string.Intern).Select(select)) {}
}

