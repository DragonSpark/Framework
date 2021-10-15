using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Runtime.Objects;

public sealed class Projection : ReadOnlyDictionary<string, object>, IProjection
{
	readonly string _text;

	public Projection(string text, Type instanceType, IDictionary<string, object> properties) : base(properties)
	{
		_text        = text;
		InstanceType = instanceType;
	}

	public override string ToString() => _text;

	public Type InstanceType { get; }
}

public class Projection<T> : ISelect<T, IProjection> where T : notnull
{
	readonly Func<T, string>                      _formatter;
	readonly Func<T, IDictionary<string, object>> _properties;

	public Projection(ISelect<T, string> formatter, params Expression<Func<T, object>>[] expressions)
		: this(formatter.Get, expressions) {}

	public Projection(Func<T, string> formatter, params Expression<Func<T, object>>[] expressions)
		: this(formatter, new Values(expressions.Select(x => new Property<T>(x)).Result<IProperty<T>>())
		                  .Select(x => x.ToOrderedDictionary())
		                  .Get) {}

	public Projection(Func<T, string> formatter, Func<T, IDictionary<string, object>> properties)
	{
		_formatter  = formatter;
		_properties = properties;
	}

	public IProjection Get(T parameter)
		=> new Projection(_formatter(parameter), parameter.GetType(), _properties(parameter));

	sealed class Values : ISelect<T, IEnumerable<Pair<string, object>>>
	{
		readonly Array<IProperty<T>> _properties;

		public Values(Array<IProperty<T>> properties) => _properties = properties;

		public IEnumerable<Pair<string, object>> Get(T parameter)
		{
			foreach (var property in _properties.Open())
			{
				yield return property.Get(parameter);
			}
		}
	}
}