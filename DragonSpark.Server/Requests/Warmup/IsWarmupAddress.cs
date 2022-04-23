using DragonSpark.Compose;
using DragonSpark.Compose.Extents.Selections;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace DragonSpark.Server.Requests.Warmup;

sealed class IsWarmupAddress : AnyCondition<HttpContext>
{
	public static IsWarmupAddress Default { get; } = new();

	IsWarmupAddress() : this(Start.A.Selection<HttpContext>()) {}

	IsWarmupAddress(SelectionExtent<HttpContext> extent)
		: this(extent.By.Calling(x => x.Connection.RemoteIpAddress.Verify())) {}

	IsWarmupAddress(Selector<HttpContext, IPAddress> address)
		: base(address.Select(IsLocalHostConnection.Default), address.Select(IsInternalAddress.Default)) {}
}