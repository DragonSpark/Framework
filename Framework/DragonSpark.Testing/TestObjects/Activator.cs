using DragonSpark.Activation;
using System;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.TypeSystem;
using DragonSpark.Windows;

namespace DragonSpark.Testing.TestObjects
{
	public class AssemblyProvider : AssemblyProviderBase
	{
		public static AssemblyProvider Instance { get; } = new AssemblyProvider();

		protected override Assembly[] DetermineAll()
		{
			var result = Assembly.GetExecutingAssembly().Append( new[] { typeof(AssemblyProviderBase), typeof(Tests), typeof(Process) }.Select( type => type.Assembly ) ).ToArray();
			return result;
		}
	}

	public class Activator : IActivator
	{
		public bool CanActivate( Type type, string name )
		{
			return true;
		}

		public object Activate( Type type, string name = null )
		{
			object result = type == typeof(Object) ? new Object { Name = name ?? "DefaultActivation" } : null;
			return result;
		}

		public bool CanConstruct( Type type, params object[] parameters )
		{
			return true;
		}

		public object Construct( Type type, params object[] parameters )
		{
			var result = type == typeof(Item) ? new Item { Parameters = parameters } : null;
			return result;
		}
	}
}