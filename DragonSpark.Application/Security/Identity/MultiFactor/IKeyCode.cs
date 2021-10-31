using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity.MultiFactor;

public interface IKeyCode<T> : ISelecting<UserInput<T>, KeyCodeView> where T : class {}