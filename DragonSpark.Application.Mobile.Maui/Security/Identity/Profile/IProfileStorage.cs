using DragonSpark.Application.Security.Identity.Profile;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity.Profile;

public interface IProfileStorage<T> : DragonSpark.Model.Operations.Results.Stop.IStopAware<T?> where T : ProfileBase;