namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public sealed class FocusOnVisibleBehavior : ExecuteOnVisibleBehaviorBase
{
    protected override void Execute()
    {
        TargetElement?.Focus();
    }
}