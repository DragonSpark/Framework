using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Security.Claims;

namespace DragonSpark.Presentation.Connections.Circuits;

public sealed record CircuitRecord(Circuit Subject, NavigationManager Navigation, ClaimsPrincipal User,
                                   string? Referrer);