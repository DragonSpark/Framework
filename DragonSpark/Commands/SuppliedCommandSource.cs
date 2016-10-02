using DragonSpark.Sources;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace DragonSpark.Commands
{
	public class SuppliedCommandSource : ItemSource<ICommand>, ICommandSource
	{
		protected SuppliedCommandSource() {}
		public SuppliedCommandSource( params ICommandSource[] sources ) : this( sources.Concat() ) {}
		public SuppliedCommandSource( IEnumerable<ICommand> items ) : base( items ) {}
		public SuppliedCommandSource( params ICommand[] items ) : base( items ) {}
	}
}