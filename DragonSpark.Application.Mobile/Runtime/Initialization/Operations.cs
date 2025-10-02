using System.Collections.Generic;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

sealed class Operations : Variable<List<IOperation>>
{
    public static Operations Default { get; } = new();

    Operations() : base([]) {}
}