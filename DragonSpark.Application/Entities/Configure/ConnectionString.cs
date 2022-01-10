using DragonSpark.Text;
using Microsoft.Extensions.Configuration;

namespace DragonSpark.Application.Entities.Configure;

sealed class ConnectionString<T> : IFormatter<IConfiguration>
{
	public static ConnectionString<T> Default { get; } = new ConnectionString<T>();

	ConnectionString() : this(ConnectionName<T>.Default) {}

	readonly string _name;

	public ConnectionString(string name) => _name = name;

	public string Get(IConfiguration parameter) => parameter.GetConnectionString(_name);
}