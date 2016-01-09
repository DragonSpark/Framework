using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.TypeSystem;
using DragonSpark.Windows.Runtime;
using PostSharp.Patterns.Contracts;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Testing.Objects
{
	public class AssemblyProvider : AssemblySourceBase, IAssemblyProvider
	{
		readonly Func<Assembly> domain;

		public AssemblyProvider() : this( DomainApplicationAssemblyLocator.Instance.Create ) {}

		public AssemblyProvider( [Required]Func<Assembly> domain )
		{
			this.domain = domain;
		}

		protected override Assembly[] CreateItem() => domain().Append( new[] { typeof( AssemblySourceBase ), typeof( Class ), typeof( Tests ), typeof( BindingOptions ) }.Select( type => type.Assembly ) ).Distinct().ToArray();
	}
}