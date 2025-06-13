namespace DragonSpark.Application.Mobile.Maui.Navigation;

public sealed class GoBack : Go
{
    public static GoBack Default { get; } = new();

    GoBack() : base("..") {}
}