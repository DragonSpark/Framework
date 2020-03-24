using DragonSpark.Model.Selection;
using Microsoft.Extensions.Configuration;

namespace DragonSpark.Application.Compose.Entities
{
	sealed class ConnectionString<T> : ISelect<IConfiguration, string>
	{
		public static ConnectionString<T> Default { get; } = new ConnectionString<T>();

		ConnectionString() : this(ConnectionName<T>.Default) {}

		readonly string _name;

		public ConnectionString(string name) => _name = name;

		public string Get(IConfiguration parameter) => parameter.GetConnectionString(_name);
	}
}