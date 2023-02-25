using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime;

public readonly record struct Throttle<T>(T Parameter, TaskCompletionSource Source);

