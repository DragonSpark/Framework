using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Reflection.Types;

sealed class Interfaces : ISelect<TypeInfo, IEnumerable<Type>>, IActivateUsing<Type>
{
	public static Interfaces Default { get; } = new Interfaces();

	Interfaces() : this(x => Default.Get(x)) {}

	readonly Func<Type, IEnumerable<Type>> _selector;

	public Interfaces(Func<Type, IEnumerable<Type>> selector) => _selector = selector;

	public IEnumerable<Type> Get(TypeInfo parameter)
		=> ExtensionMethods.Prepend(parameter.ImplementedInterfaces.SelectMany(_selector), parameter);
}