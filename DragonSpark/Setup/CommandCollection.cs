using System.Collections.Generic;
using System.Windows.Input;
using DragonSpark.Extensions;

namespace DragonSpark.Setup
{
	public class CommandCollection : Runtime.Collection<ICommand>
	{
		public CommandCollection()
		{}

		public CommandCollection( IEnumerable<ICommand> collection ) : base( collection.Fixed() )
		{}
	}
}