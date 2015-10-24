using DragonSpark.Extensions;
using System;
using System.Reflection;
using System.Windows.Markup;

namespace DragonSpark.Runtime
{
	public abstract class AssemblyProviderBase : IAssemblyProvider
	{
		readonly Lazy<Assembly[]> all;

		protected AssemblyProviderBase()
		{
			all = new Lazy<Assembly[]>( DetermineAll );
		}

		protected abstract Assembly[] DetermineAll();
		
		public Assembly[] GetAssemblies()
		{
			var result = all.Value;
			return result;
		}
	}

	public class Collection : Collection<object>
	{}

	[Ambient]
	public class Collection<T> : System.Collections.ObjectModel.Collection<T> where T : class
	{
		protected override void InsertItem( int index, T item )
		{
			var prepared = Prepare( item );
			base.InsertItem( index, prepared );
		}

		protected virtual T Prepare( T command )
		{
			return command.WithDefaults();
		}
	}
}