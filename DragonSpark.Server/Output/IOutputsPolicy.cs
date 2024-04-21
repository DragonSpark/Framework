using DragonSpark.Text;
using Microsoft.AspNetCore.OutputCaching;

namespace DragonSpark.Server.Output;

public interface IOutputsPolicy : IOutputCachePolicy, IText;