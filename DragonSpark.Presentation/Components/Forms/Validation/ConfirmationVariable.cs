using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Results;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed class ConfirmationVariable : Variable<IResulting<bool>>, IResulting<bool>
{
	public new ValueTask<bool> Get() => base.Get().Verify().Get();
}