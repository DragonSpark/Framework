using System.Collections.Generic;
using DragonSpark.Model.Commands;

namespace DragonSpark.Model.Sequences.Collections.Commands
{
	class AddCommand<T> : Command<T>
	{
		public AddCommand(ICollection<T> collection) : base(collection.Add) {}
	}
}