using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime;

public readonly record struct Throttle<T>(T Parameter, Operate<T> callback, TaskCompletionSource Source);

