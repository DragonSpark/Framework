using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Authentication;

namespace DragonSpark.Server.Mobile.Security.Devices;

public interface IAuthenticateDevice : IStopAware<AuthenticateDeviceInput, AuthenticateResult>;