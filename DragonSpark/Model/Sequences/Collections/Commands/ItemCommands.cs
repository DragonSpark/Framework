using System.Collections.Generic;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Model.Sequences.Collections.Commands
{
	sealed class ItemCommands<T> : Store<IList<T>, ICommand<T>>
	{
		public static ItemCommands<T> Default { get; } = new ItemCommands<T>();

		ItemCommands() : base(AddItemCommands<T>.Default.Select(InsertItemCommands<T>.Default).Get) {}
	}
}