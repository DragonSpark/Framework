using System.Collections.Generic;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Model.Validation;
internal class Class1;


public interface IValidationAware : ICommand<ValidationModelRecord>, IResult<IValidationModel>;

public readonly record struct ValidationModelRecord(Dictionary<string, string[]> Local, Dictionary<string, string[]> External);

public interface IValidationModel : ICondition
{
    bool IsValid { get; set; }
    Dictionary<string, string[]> Local { get; }
    Dictionary<string, string[]> External { get; }
}