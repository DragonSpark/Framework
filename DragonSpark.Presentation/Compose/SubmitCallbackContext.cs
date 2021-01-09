using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.Forms.Validation;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Compose
{
	public class SubmitCallbackContext : CallbackContext<EditContext>
	{
		public SubmitCallbackContext(Func<EditContext, Task> method)
			: this(method, method.Start().Then().Structure().Out()) {}

		public SubmitCallbackContext(Func<EditContext, Task> method, IOperation<EditContext> operation)
			: base(method.Target, new Submit(operation).Then().Allocate()) {}
	}
}