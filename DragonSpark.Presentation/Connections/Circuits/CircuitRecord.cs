using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Security.Claims;

namespace DragonSpark.Presentation.Connections.Circuits;

public sealed record CircuitRecord(string Identifier, Circuit Subject, NavigationManager Navigation,
                                   ClaimsPrincipal User);