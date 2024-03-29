﻿using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public readonly record struct FieldContext(EditContext Context, FieldIdentifier Identifier);