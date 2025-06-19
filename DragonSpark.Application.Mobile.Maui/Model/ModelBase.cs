using CommunityToolkit.Mvvm.ComponentModel;
using DragonSpark.Application.Mobile.Model;

namespace DragonSpark.Application.Mobile.Maui.Model;

public partial class ModelBase : ObservableObject, IActivityAware
{
    [ObservableProperty]
    public partial bool IsActive { get; set; }

    public void Execute(bool parameter)
    {
        IsActive = parameter;
    }
}