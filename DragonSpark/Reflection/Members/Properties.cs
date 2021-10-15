using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

public class Properties<T> : Properties<T, PropertyInfo> where T : Attribute
{
	public static Properties<T> Default { get; } = new Properties<T>();

	Properties() : base(x => x.Metadata) {}
}

public class Properties<TAttribute, T> : ISelect<Type, Array<T>> where TAttribute : Attribute
{
	readonly Func<Property<TAttribute>, T?> _select;
	readonly BindingFlags                   _flags;

	protected Properties(Func<Property<TAttribute>, T?> select) : this(@select, AllInstanceFlags.Default) {}

	protected Properties(Func<Property<TAttribute>, T?> select, BindingFlags flags)
	{
		_select = @select;
		_flags  = flags;
	}

	public Array<T> Get(Type parameter)
	{
		var       properties = parameter.GetTypeInfo().GetProperties(_flags);
		using var builder    = ArrayBuilder.New<T>(properties.Length);
		for (var i = 0; i < properties.Length; i++)
		{
			var property  = properties[i];
			var attribute = property.Attribute<TAttribute>();
			if (attribute != null)
			{
				var item = _select(new(property, attribute));
				if (item is not null)
				{
					builder.UncheckedAdd(item);
				}
			}
		}

		return builder;
	}
}