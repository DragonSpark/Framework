using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace DragonSpark.Application.Runtime;

public sealed record ThrottleContext(Timer Subject, HashSet<TaskCompletionSource> Sources);