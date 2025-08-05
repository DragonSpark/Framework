using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Model.Validation;

public interface IValidationAware : ICommand<ValidationModelRecord>, IResult<IValidationModel>;