using DragonSpark.Application.Mobile.Maui.Run;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Mobile.Maui;

sealed class Configure<T> : IAlteration<BuildHostContext>
{
    public static Configure<T> Default { get; } = new();

    Configure() {}

    public BuildHostContext Get(BuildHostContext parameter)
        => parameter.Configure(ApplyApplicationConfiguration<T>.Default.Adapt(),
                               Application.DefaultRegistrations.Default, DefaultRegistrations.Default);
}