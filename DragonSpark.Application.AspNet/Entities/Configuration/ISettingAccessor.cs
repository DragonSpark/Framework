using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Entities.Configuration;

public interface ISettingAccessor : ISelecting<string, string?>, IOperation<Pair<string, string?>>
{
	IRemove Remove { get; }
}