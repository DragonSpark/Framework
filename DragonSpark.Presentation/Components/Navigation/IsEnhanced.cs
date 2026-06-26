using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Components.Navigation;

public sealed class IsEnhanced : ICondition<HttpContext>
{
    public static IsEnhanced Default { get; } = new();

    IsEnhanced() : this(IsEnhancedValue.Default) {}
    
    readonly ICondition<string> _value;

    public IsEnhanced(ICondition<string> value) => _value = value;

    public bool Get(HttpContext parameter) => _value.Get(parameter.Request.Headers.Accept.ToString());
}