using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Composition.Hosting.Core;
using System.Linq;

namespace DragonSpark.Composition
{
	public sealed class InstanceExportDescriptorProvider<T> : ExportDescriptorProvider
	{
		readonly T instance;
		readonly CompositionContract[] contracts;
		readonly CompositeActivator activate;
		readonly Func<IEnumerable<CompositionDependency>, ExportDescriptor> get;

		public InstanceExportDescriptorProvider( T instance, string name = null )
		{
			this.instance = instance;
			contracts = typeof(T).Append( instance.GetType() ).Distinct().Introduce( name, tuple => new CompositionContract( tuple.Item1, tuple.Item2 ) ).ToArray();
			activate = Activator;
			get = GetDescriptor;
		}

		public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors( CompositionContract contract, DependencyAccessor descriptorAccessor )
		{
			if ( contracts.Contains( contract ) )
			{
				yield return new ExportDescriptorPromise( contract, GetType().FullName, true, NoDependencies, get );
			}
		}

		ExportDescriptor GetDescriptor( IEnumerable<CompositionDependency> dependencies ) => ExportDescriptor.Create( activate, NoMetadata );

		object Activator( LifetimeContext context, CompositionOperation operation ) => instance;
	}
}