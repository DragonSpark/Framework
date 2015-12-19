using System.Collections.Generic;
using System.Windows.Input;
using DragonSpark.Extensions;

namespace DragonSpark.Setup
{
	public class CommandCollection : CommandCollection<ICommand>
	{}

	public class CommandCollection<T> : Runtime.Collection<T> where T : ICommand
	{
		public CommandCollection()
		{}

		public CommandCollection( IEnumerable<T> collection ) : base( collection.Fixed() )
		{}
	}
}