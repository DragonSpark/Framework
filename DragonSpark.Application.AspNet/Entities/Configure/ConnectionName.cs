using DragonSpark.Compose;

namespace DragonSpark.Application.AspNet.Entities.Configure;

sealed class ConnectionName<T> : Text.Text
{
	public static ConnectionName<T> Default { get; } = new();

	ConnectionName() : base($"{A.Type<T>().Name}Connection") {}
}