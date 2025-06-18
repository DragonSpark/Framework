using System.Collections.Generic;

namespace DragonSpark.Application.Mobile.Maui.Model;

public sealed class ValidationResults : Dictionary<string, string[]>
{
    public ValidationResults() {}

    public ValidationResults(IDictionary<string, string[]> dictionary) : base(dictionary) {}
}