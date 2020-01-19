using DragonSpark.Model.Commands;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Collections.Commands
{
	class AddCommand<T> : Command<T>
	{
		public AddCommand(ICollection<T> collection) : base(collection.Add) {}
	}
}