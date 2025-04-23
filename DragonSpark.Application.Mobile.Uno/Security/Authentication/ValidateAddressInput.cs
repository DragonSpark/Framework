using System.Threading;

namespace DragonSpark.Application.Mobile.Uno.Security.Authentication;

public readonly record struct ValidateAddressInput(string Identifier, CancellationToken Token);