using System;
using System.Threading;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Runtime.Process;

/// <summary>
/// ATTRIBUTION: https://stackoverflow.com/a/60546519
/// </summary>
public sealed class FirstInstance<T> : ICondition
{
    public static FirstInstance<T> Default { get; } = new();

    FirstInstance() : this(ExistingMarker<T>.Default) {}

    readonly Mutex _marker;

    public FirstInstance(Mutex marker) => _marker = marker;

    public bool Get(None parameter) => _marker.WaitOne(TimeSpan.Zero, true);
}