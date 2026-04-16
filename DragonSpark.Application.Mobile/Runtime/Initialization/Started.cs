using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

public sealed class Started : AnyCondition<None>, ICondition
{
    public static Started Default { get; } = new();

    Started() : base(Initializing.Default, Initialized.Default) {}
}