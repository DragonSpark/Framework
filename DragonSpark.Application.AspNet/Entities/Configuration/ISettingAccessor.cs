using DragonSpark.Model;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Entities.Configuration;

public interface ISettingAccessor : IStopAware<string, string?>, IStopAware<Pair<string, string?>>
{
	IRemove Remove { get; }
}