﻿using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Presentation.Environment.Browser.Document;

public interface IFocusedElement : IAsyncDisposable
{
	IOperation Store { get; }
	IOperation Restore { get; }
}