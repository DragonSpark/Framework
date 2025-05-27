using DragonSpark.Application.Mobile.Maui.Storage;
using DragonSpark.Application.Security.Identity.Profile;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity.Profile;

public class ProfileViewStorage<T> : StorageValue<T>, IProfileStorage<T> where T : ProfileBase;