using System.Collections.Generic;
using System.Threading.Tasks;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

sealed class Operations : Variable<List<Task>>
{
    public static Operations Default { get; } = new();

    Operations() : base([]) {}
}