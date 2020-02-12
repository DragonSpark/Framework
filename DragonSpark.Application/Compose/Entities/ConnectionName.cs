using DragonSpark.Compose;

namespace DragonSpark.Application.Compose.Entities
{
	sealed class ConnectionName<T> : Text.Text
	{
		public static ConnectionName<T> Default { get; } = new ConnectionName<T>();

		ConnectionName() : base($"{A.Type<T>().Name}Connection") {}
	}
}