using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Mobile;

sealed class Configure : IAlteration<BuildHostContext>
{
    public static Configure Default { get; } = new();

    Configure() {}

    public BuildHostContext Get(BuildHostContext parameter)
        => parameter.Configure(Application.DefaultRegistrations.Default, DefaultRegistrations.Default,
                               Presentation.Models.Registrations.Default,
                               Device.Camera.Registrations.Default);
}