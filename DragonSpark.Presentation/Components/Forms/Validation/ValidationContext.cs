using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public readonly record struct ValidationContext(FieldContext Field, ValidationMessageStore Messages,
                                                FieldValidationMessages Messaging);