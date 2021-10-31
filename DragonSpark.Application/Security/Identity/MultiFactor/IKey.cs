using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity.MultiFactor;

public interface IKey<T> : ISelecting<UserInput<T>, string> where T : class {}