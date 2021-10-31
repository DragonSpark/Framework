using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed record ValidationFieldMessage(FieldIdentifier Field, string Message);