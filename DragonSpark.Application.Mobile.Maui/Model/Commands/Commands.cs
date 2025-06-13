using CommunityToolkit.Mvvm.Input;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Application.Mobile.Maui.Model.Commands;

public sealed class Commands<T> : ReferenceValueStore<IOperation<T>, IAsyncRelayCommand<T>>
{
    public static Commands<T> Default { get; } = new();

    Commands() : base(x => new Adaptor<T>(x)) {}
}