using System.Collections.Generic;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Communication.Http;

public interface IDeconstruct : ISelect<IDictionary<string, object?>, IEnumerable<KeyValuePair<string, string>>>;

public interface IDeconstruct<in T> : ISelect<T, IEnumerable<KeyValuePair<string, string>>>;