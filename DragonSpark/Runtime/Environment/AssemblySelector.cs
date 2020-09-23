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

		public AssemblySelector(Func<Array<Assembly>, Assembly[]> select) => _select = select;

		public Array<Assembly> Get(Array<Assembly> parameter) => _select(parameter).Union(parameter.Open()).Result();
	}
}