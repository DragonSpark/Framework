﻿using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Presentation.Browser;

public interface IFocusedElement : IAsyncDisposable
{
	IOperation Store { get; }
	IOperation Restore { get; }
}