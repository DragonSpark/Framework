using System.Collections.Generic;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Model.Validation;

public interface IValidationModel : ICondition
{
    bool IsValid { get; set; }
    Dictionary<string, string[]> Local { get; }
    Dictionary<string, string[]> External { get; }
}