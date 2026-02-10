using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Authentication;

namespace DragonSpark.Server.Mobile.Security.Devices.Authentication;

public interface IAuthenticateDevice : IStopAware<AuthenticateDeviceInput, AuthenticateResult>;