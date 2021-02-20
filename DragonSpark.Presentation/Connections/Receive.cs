using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Connections
{
	public class Receive : IReceive
	{
		readonly Func<HubConnection> _connection;
		readonly string              _name;

		public Receive(IResult<HubConnection> connection, string name) : this(connection.Get, name) {}

		public Receive(Func<HubConnection> connection, string name)
		{
			_connection = connection;
			_name       = name;
		}

		public IReceiver Get(Func<Task> parameter)
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

		}
	}


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

		}
	}
}