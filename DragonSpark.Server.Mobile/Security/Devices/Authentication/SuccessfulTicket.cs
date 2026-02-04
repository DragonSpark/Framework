using System.Security.Claims;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Model.Selection;
using DragonSpark.Server.Mobile.Security.Devices.Claims;
using Microsoft.AspNetCore.Authentication;

namespace DragonSpark.Server.Mobile.Security.Devices.Authentication;

sealed class SuccessfulTicket : ISelect<SuccessfulTicketInput, AuthenticateResult>
{
    public static SuccessfulTicket Default { get; } = new();

    SuccessfulTicket() : this(new(ClaimTypes.AuthenticationMethod, SchemeName.Default), DeviceClaimName.Default) {}

    readonly Claim  _method;
    readonly string _name;

    public SuccessfulTicket(Claim method, string name)
    {
        _method = method;
        _name   = name;
    }

    public AuthenticateResult Get(SuccessfulTicketInput parameter)
    {
        var (device, scheme) = parameter;
        var identity = new ClaimsIdentity([new(_name, device), _method], scheme);
        return AuthenticateResult.Success(new(new(identity), scheme));
    }
}