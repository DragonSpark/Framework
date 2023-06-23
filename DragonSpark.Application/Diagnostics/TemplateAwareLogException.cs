﻿using DragonSpark.Model.Operations.Selection;
using System;

namespace DragonSpark.Application.Diagnostics;

sealed class TemplateAwareLogException : Coalesce<LogExceptionInput, Exception>, ILogException
{
	public TemplateAwareLogException(ILogException previous) : base(LogTemplateException.Default, previous) {}
}