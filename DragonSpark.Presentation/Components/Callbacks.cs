using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	public sealed class Callbacks
	{
		readonly IExceptions _exceptions;

		public Callbacks(IExceptions exceptions) => _exceptions = exceptions;

		public EventCallback<MouseEventArgs> Click(Callback<MouseEventArgs> parameter) => Get(parameter);

		public EventCallback Get(Func<Task> parameter) => Get(parameter.Target, parameter);

		public EventCallback Get(object receiver, Func<Task> parameter)
			=> EventCallback.Factory.Create(receiver, new ExceptionAwareCallback(receiver.GetType(),
			                                                                     _exceptions, parameter).Promote);

		public EventCallback<TIn> Get<TIn>(Callback<TIn> parameter) => Get(parameter.Target, parameter);

		public EventCallback<TIn> Get<TIn>(object receiver, Callback<TIn> parameter)
		{
			var callback = new ExceptionAwareCallback<TIn>(receiver.GetType(), _exceptions, parameter);
			var handler  = new Func<TIn, Task>(callback.Promote);
			var result   = EventCallback.Factory.Create(receiver, handler);
			return result;
		}
	}
}