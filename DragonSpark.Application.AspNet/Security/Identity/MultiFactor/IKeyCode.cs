using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.AspNet.Security.Identity.MultiFactor;

public interface IKeyCode<T> : ISelecting<UserInput<T>, KeyCodeView> where T : class;