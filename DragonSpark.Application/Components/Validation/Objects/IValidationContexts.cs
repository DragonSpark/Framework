using System.ComponentModel.DataAnnotations;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Components.Validation.Objects;

public interface IValidationContexts : ISelect<NewValidationContext, ValidationContext>,
                                       ISelect<ValidationContext, GraphValidationContext>;