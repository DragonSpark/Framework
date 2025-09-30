using System;
using System.Collections.Generic;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

sealed class Commands : Variable<List<Action>>
{
    public static Commands Default { get; } = new();

    Commands() : base([]) {}
}