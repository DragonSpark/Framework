using DragonSpark.Extensions;
using System.Collections.Generic;
using System.Windows.Input;

namespace DragonSpark.Setup
{
	public class CommandCollection : CommandCollection<ICommand>
	{}

	public class CommandCollection<T> : Runtime.Collection<T> where T : class, ICommand
	{
		public CommandCollection()
		{}

		public CommandCollection( IEnumerable<T> collection ) : base( collection.Fixed() )
		{}
	}
}