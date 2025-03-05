using System.Threading;

namespace DragonSpark.Application.Mobile.Security.Authentication;

public readonly record struct ValidateAddressInput(string Identifier, CancellationToken Token);