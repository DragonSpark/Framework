using DragonSpark.Application.Model;
using DragonSpark.Contracts.Uploads;
using DragonSpark.Text;

namespace DragonSpark.Application.AspNet.Runtime;

public interface ITemporaryPath : IFormatter<UserInput<WorkspacePath>>;