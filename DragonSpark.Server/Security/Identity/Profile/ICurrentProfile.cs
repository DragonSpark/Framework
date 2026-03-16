using DragonSpark.Application.Security.Identity.Profile;
using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Server.Security.Identity.Profile;

public interface ICurrentProfile : IStopAware<ProfileBase>;