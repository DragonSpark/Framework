using System.Collections.Generic;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

public interface ICommands<T> : IMutable<List<T>?>, ICommand;