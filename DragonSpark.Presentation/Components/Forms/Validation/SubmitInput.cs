using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public readonly record struct SubmitInput<T>(EditContext Context, T Subject);