using System.Runtime.CompilerServices;

namespace DragonSpark.Model.Operations.Allocated;

public delegate ConfiguredTaskAwaitable Await<in T>(T parameter);

public delegate ConfiguredTaskAwaitable Await();

public delegate ConfiguredTaskAwaitable<TOut> Await<in TIn, TOut>(TIn parameter);