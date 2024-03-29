﻿using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.Forms;

public class TextInputComponent : ComponentBase
{
	[Parameter]
    public bool ValidationDisabled { get; set; }

    [Parameter]
    public ForwardRef RefBack { get; set; } = new();

    [Parameter]
    public string Id { get; set; } = IdGeneratorHelper.Generate("matBlazor_id_");

    [Parameter]
    public IDictionary<string, object> InputAttributes { get; set; } = default!;

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> Attributes { get; set; } = default!;

    [Parameter]
    public string Class { get; set; } = default!;

    [Parameter]
    public string Style { get; set; } = default!;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public EventCallback<MouseEventArgs> IconOnClick { get; set; }

    [Parameter]
    public EventCallback<FocusEventArgs> OnFocus { get; set; }

    [Parameter]
    public EventCallback<FocusEventArgs> OnFocusOut { get; set; }

    [Parameter]
    public EventCallback<KeyboardEventArgs> OnKeyPress { get; set; }

    [Parameter]
    public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }

    [Parameter]
    public EventCallback<KeyboardEventArgs> OnKeyUp { get; set; }

    [Parameter]
    public EventCallback<ChangeEventArgs> OnInput { get; set; }

    [Parameter]
    public string Label { get; set; } = default!;

    [Parameter]
    public string Icon { get; set; } = default!;

    [Parameter]
    public bool IconTrailing { get; set; }

    [Parameter]
    public bool Box { get; set; }

    [Parameter]
    public bool TextArea { get; set; }

    [Parameter]
    public bool Dense { get; set; }

    [Parameter]
    public bool Outlined { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public bool FullWidth { get; set; }

    [Parameter]
    public bool Required { get; set; }

    [Parameter]
    public string HelperText { get; set; } = default!;

    [Parameter]
    public bool HelperTextPersistent { get; set; }

    [Parameter]
    public bool HelperTextValidation { get; set; }

    [Parameter]
    public string PlaceHolder { get; set; } = default!;

    [Parameter]
    public bool HideClearButton { get; set; }

    [Parameter]
    public string Type { get; set; } = "text";

    [Parameter]
    public string InputClass { get; set; } = default!;

    [Parameter]
    public string InputStyle { get; set; } = default!;

    [Parameter]
    public string Format { get; set; } = default!;
}