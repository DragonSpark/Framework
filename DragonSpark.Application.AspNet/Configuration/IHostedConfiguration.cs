using System.Collections.Generic;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Configuration;

public interface IHostedConfiguration
    : ISelect<IEnumerable<KeyValuePair<string, object?>>, IReadOnlyDictionary<string, object?>>;