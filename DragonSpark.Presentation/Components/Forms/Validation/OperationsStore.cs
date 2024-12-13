using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using Microsoft.AspNetCore.Components.Forms;
using NetFabric.Hyperlinq;

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