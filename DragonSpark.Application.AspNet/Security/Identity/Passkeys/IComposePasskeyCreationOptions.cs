using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public interface IComposePasskeyCreationOptions<T> : ISelecting<ComposePasskeyCreationOptionsInput<T>, string>
    where T : class;