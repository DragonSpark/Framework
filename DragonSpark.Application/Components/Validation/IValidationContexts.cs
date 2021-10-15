using DragonSpark.Model.Selection;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Components.Validation;

public interface IValidationContexts : ISelect<NewValidationContext, ValidationContext>,
                                       ISelect<ValidationContext, GraphValidationContext> {}