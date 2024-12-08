using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Security.Identity;

public interface IUsers<T> : IResult<UsersSession<T>> where T : class;