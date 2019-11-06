using System.Collections.Generic;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Collections.Commands
{
	sealed class AddItemCommands<T> : Select<IList<T>, ICommand<T>>
	{
		public static AddItemCommands<T> Default { get; } = new AddItemCommands<T>();

		AddItemCommands() : base(key => new AddCommand<T>(key)) {}
	}
}