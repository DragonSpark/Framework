using DragonSpark.Application.Model;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Server.Mobile.Notifications;

public interface IDeviceRegistration : IStopAware<UserInput<DeviceRegistrationInput>>;