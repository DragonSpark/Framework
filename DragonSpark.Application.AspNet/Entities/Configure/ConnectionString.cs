using DragonSpark.Compose;
using DragonSpark.Text;
using Microsoft.Extensions.Configuration;

namespace DragonSpark.Application.AspNet.Entities.Configure;

public sealed class ConnectionString<T> : ConnectionString
{
	public static ConnectionString<T> Default { get; } = new();

	ConnectionString() : base(ConnectionName<T>.Default) {}
}

public class ConnectionString : IFormatter<IConfiguration>
{
	readonly string _name;

	public ConnectionString(string name) => _name = name;

	public string Get(IConfiguration parameter)
		=> parameter.GetConnectionString(_name).Verify($"Connection string {_name} not found!");
}