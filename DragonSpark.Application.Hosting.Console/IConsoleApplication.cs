using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using JetBrains.Annotations;

namespace DragonSpark.Application.Hosting.Console;

[UsedImplicitly]
public interface IConsoleApplication : ICommand<Array<string>>;