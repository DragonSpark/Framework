using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	sealed class ValidContext : IDepending<EditContext>
	{
		static async ValueTask Process(IOperations list)
		{
			if (A.Condition(list).Get())
			{
				var awaitable = list.Await();
				list.Execute();
				await awaitable;
			}
		}

		public static ValidContext Default { get; } = new ValidContext();

		ValidContext() : this(OperationsStore.Default.Get) {}

		readonly Func<EditContext, IOperations> _list;

		public ValidContext(Func<EditContext, IOperations> list) => _list = list;

		public async ValueTask<bool> Get(EditContext parameter)
		{
			var list = _list(parameter);
			if (parameter.Validate())
			{
				await Process(list).ConfigureAwait(false);

				return parameter.IsValid();
			}

			if (A.Condition(list).Get())
			{
				list.Execute();
			}

			return false;
		}
	}
}