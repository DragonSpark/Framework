using System;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Mobile.Uno.Security.Authentication;

sealed class ParseQuery : ISelect<Uri, string?>
{
    public static ParseQuery Default { get; } = new();

    ParseQuery() {}

    public string? Get(Uri parameter) => System.Web.HttpUtility.ParseQueryString(parameter.Query)["state"];
}