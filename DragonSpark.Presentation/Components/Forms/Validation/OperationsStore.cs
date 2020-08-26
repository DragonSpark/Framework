using DragonSpark.Model.Selection.Stores;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	sealed class OperationsStore : ReferenceValueStore<EditContext, IOperations>, IOperationsStore
	{
		public static OperationsStore Default { get; } = new OperationsStore();

		OperationsStore() : base(_ => new Operations()) {}
	}
}