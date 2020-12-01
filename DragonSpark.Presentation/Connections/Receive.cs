using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Connections
{
	public class Receive<T> : IReceive<T>
	{
		readonly Func<HubConnection> _connection;
		readonly string              _name;

		public Receive(IResult<HubConnection> connection, string name) : this(connection.Get, name) {}

		public Receive(Func<HubConnection> connection, string name)
		{
			_connection = connection;
			_name       = name;
		}

		public IReceiver Get(Func<T, Task> parameter)
		{
			var connection = _connection();
			var disposable = connection.On(_name, parameter);
			var result     = new Result(connection, disposable);
			return result;
		}

		sealed class Result : IReceiver
		{
			readonly HubConnection _connection;
			readonly IDisposable   _disposable;

			public Result(HubConnection connection, IDisposable disposable)
			{
				_connection = connection;
				_disposable = disposable;
			}

			public ValueTask DisposeAsync()
			{
				_disposable.Dispose();
				return _connection.DisposeAsync();
			}

			public ValueTask Get() => _connection.StartAsync().ToOperation();

			/*public async ValueTask Get()
			{
				await _connection.StartAsync();
				await _connection.SendAsync("UpdateProcess", new Guid("bac1f79e-1f95-4641-625f-08d8954e705b"));
			}*/
		}
	}
}