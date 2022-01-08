using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Configuration;

namespace DragonSpark.Composition;

sealed class Section<T> : ISelect<IConfiguration, T> where T : class
{
	public static Section<T> Default { get; } = new ();

	Section() : this(A.Type<T>().Name) {}

	readonly string _name;

	public Section(string name) => _name = name;

	public T Get(IConfiguration parameter) => parameter.GetSection(_name).Get<T>();
}