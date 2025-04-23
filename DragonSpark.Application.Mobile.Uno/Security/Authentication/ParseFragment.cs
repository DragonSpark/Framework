using System;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Mobile.Uno.Security.Authentication;

sealed class ParseFragment : ISelect<Uri, string?>
{
    public static ParseFragment Default { get; } = new();

    ParseFragment() {}

    public string? Get(Uri parameter)
    {
        var fragment   = parameter.Fragment;
        var content    = fragment.StartsWith("#") ? fragment.Substring(1) : fragment;
        var collection = System.Web.HttpUtility.ParseQueryString(content);
        return collection["state"];
    }
}