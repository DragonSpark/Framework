using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Stores;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Collections.Commands
{
	sealed class ItemCommands<T> : Store<IList<T>, ICommand<T>>
	{
		public static ItemCommands<T> Default { get; } = new ItemCommands<T>();

		ItemCommands() : base(AddItemCommands<T>.Default.Then().Introduce().Select(InsertItemCommands<T>.Default)) {}
	}
}