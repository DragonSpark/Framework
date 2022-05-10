using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public readonly record struct ValidatedField(in FieldIdentifier Identifier, bool Valid);