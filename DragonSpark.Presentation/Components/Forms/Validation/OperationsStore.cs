using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Conditions;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using Microsoft.AspNetCore.Components.Forms;
using NetFabric.Hyperlinq;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation;

sealed class OperationsStore : ReferenceValueStore<EditContext, IOperations>, IOperationsStore
{
	public static OperationsStore Default { get; } = new();

	OperationsStore() : base(_ => new Operations()) { }
}

// TODO

sealed class IsValid : Condition<EditContext>
{
	public static IsValid Default { get; } = new();

	IsValid() : base(x => !x.GetValidationMessages().AsValueEnumerable().Any()) { }
}

sealed class ValidateCompletely : IDepending<EditContext>
{
	public static ValidateCompletely Default { get; } = new();

	ValidateCompletely() : this(OperationsStore.Default) { }

	readonly IOperationsStore _operations;

	public ValidateCompletely(IOperationsStore operations) => _operations = operations;

	public async ValueTask<bool> Get(EditContext parameter)
	{
		var valid = parameter.Validate();
		if (valid)
		{
			await _operations.Get(parameter).Await();
		}
		return valid && parameter.IsValid();
	}
}