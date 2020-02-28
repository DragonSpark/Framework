using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms
{
	public interface IValidation : IOperationResult<FieldIdentifier, bool> {}
}