using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public interface IAddPasskey : IStopAware<AddPasskeyInput, AddPasskeyResult>;