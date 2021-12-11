using Syncfusion.Blazor;

namespace DragonSpark.Syncfusion.Components;

public class SfDataManager : global::Syncfusion.Blazor.Data.SfDataManager
{
	public SfDataManager() => Adaptor = Adaptors.CustomAdaptor;
}