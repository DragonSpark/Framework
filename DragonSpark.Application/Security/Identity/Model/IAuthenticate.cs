using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity.Model
{
	public interface IAuthenticate<T> : IOperation<Login<T>> {}
}