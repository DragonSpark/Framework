using System.Collections.Generic;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Maui.Model;

public sealed record ValidationResults(Dictionary<string, string[]> Local, Dictionary<string, string[]> External): ICondition
{
    public ValidationResults() : this([], []) {}

    public bool Get(None parameter) => Local.Count + External.Count == 0;
}