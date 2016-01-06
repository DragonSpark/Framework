using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Objects;
using DragonSpark.TypeSystem;
using DragonSpark.Windows.Runtime;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Windows.Testing.TestObjects
{
	public class AssemblyProvider : AssemblySourceBase, IAssemblyProvider
	{
		public static AssemblyProvider Instance { get; } = new AssemblyProvider();

		protected override Assembly[] CreateItem()
		{
			var result = Assembly.GetExecutingAssembly().Append( new[] { typeof(AssemblySourceBase), typeof(Class), typeof(Tests), typeof(BindingOptions) }.Select( type => type.Assembly ) ).ToArray();
			return result;
		}
	}
}
