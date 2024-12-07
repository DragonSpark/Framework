using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

public readonly record struct Report<T>(T Input, Action<Task> Subject);