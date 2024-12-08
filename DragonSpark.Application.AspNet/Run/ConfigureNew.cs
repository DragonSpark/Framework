﻿using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Application.AspNet.Run;

public record ConfigureNew<TIn, TOut>(Func<TIn, TOut> New, ICommand<TOut> Configure);