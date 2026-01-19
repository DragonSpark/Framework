using System;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

sealed class ShouldIntercept : ICondition<HttpRequest>
{
    readonly PathString       _path;
    readonly StringComparison _comparison;

    public ShouldIntercept(PasskeySettings settings) : this(settings.LoginPath, StringComparison.OrdinalIgnoreCase) {}

    public ShouldIntercept(PathString path, StringComparison comparison)
    {
        _path       = path;
        _comparison = comparison;
    }

    public bool Get(HttpRequest parameter)
        => HttpMethods.IsPost(parameter.Method) && parameter.Path.Equals(_path, _comparison);
}