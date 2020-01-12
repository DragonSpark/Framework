using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Runtime.Environment
{
	sealed class Assemblies : ArrayStore<Assembly>
	{
		public static Assemblies Default { get; } = new Assemblies();

		Assemblies() : this(AssemblySelector.Default) {}

		public Assemblies(ISelect<Array<Assembly>, Array<Assembly>> select) : this(select, DefaultAssemblies.Default) {}

		public Assemblies(ISelect<Array<Assembly>, Array<Assembly>> select, IArray<Assembly> parameter)
			: base(select.Then().Subject.Bind(parameter)) {}
	}

	sealed class EnvironmentAwareAssemblies : IArray<string, Assembly>
	{
		public static EnvironmentAwareAssemblies Default { get; } = new EnvironmentAwareAssemblies();

		EnvironmentAwareAssemblies() {}

		public Array<Assembly> Get(string parameter)
		{
			var names = new ComponentAssemblyNames(new SpecificEnvironmentAssemblyName(parameter),
			                                       EnvironmentAssemblyName.Default);
			var selector = new AssemblySelector(names);
			var result   = new Assemblies(selector).Get().Open();
			return result;
		}
	}

	sealed class AssemblySelector : IAlteration<Array<Assembly>>,
	                                IActivateUsing<ISelect<Array<Assembly>, Array<Assembly>>>
	{
		public static AssemblySelector Default { get; } = new AssemblySelector();

		AssemblySelector() : this(ComponentAssemblyNames.Default) {}

		readonly Func<Array<Assembly>, IEnumerable<Assembly>> _select;

		[UsedImplicitly]
		public AssemblySelector(ISelect<AssemblyName, IEnumerable<AssemblyName>> names)
			: this(Start.A.Selection.Of.Type<Assembly>()
			            .As.Sequence.Array.By.Self.Select(new AssemblySelectorQuery(names.Get))) {}

		public AssemblySelector(Func<Array<Assembly>, IEnumerable<Assembly>> select) => _select = select;

		public Array<Assembly> Get(Array<Assembly> parameter)
			=> _select(parameter)?.Union(parameter.Open()).Result() ?? throw new InvalidOperationException();
	}

	sealed class DefaultAssemblies : ArrayInstance<Assembly>
	{
		public static DefaultAssemblies Default { get; } = new DefaultAssemblies();

		DefaultAssemblies() : base(PrimaryAssembly.Default, HostingAssembly.Default) {}
	}
}