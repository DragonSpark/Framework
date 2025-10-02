using System.Collections.Generic;
using System.Threading.Tasks;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

sealed class Tasks : Variable<List<Task>>
{
    public static Tasks Default { get; } = new();

    Tasks() : base([]) {}
}