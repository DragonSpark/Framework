using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity.Profile;

public interface IUserReference<T> : IAltering<T> where T : class {}