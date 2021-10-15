using DragonSpark.Model.Results;

namespace DragonSpark.Application.Security.Identity;

public interface IUsers<T> : IResult<UsersSession<T>> where T : class {}