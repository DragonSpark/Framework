using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed class ConfirmationVariable : Variable<IResulting<bool>>, IResulting<bool>
{
	public new ValueTask<bool> Get() => base.Get().Get();
}