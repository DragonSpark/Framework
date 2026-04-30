using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

public interface IFormatUserData<T> : ISelecting<ComposeUserDataInput<T>, Array<byte>> where T : class;