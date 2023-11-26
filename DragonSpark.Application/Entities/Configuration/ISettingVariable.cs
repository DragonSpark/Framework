using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Application.Entities.Configuration;

public interface ISettingVariable : IResulting<string?>, IOperation<string>
{
	IOperation Remove { get; }
}