using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class ComposeIdentityProperties : ISelect<ITypeBase, ImmutableList<IProperty>>
{
	public static ComposeIdentityProperties Default { get; } = new();

	ComposeIdentityProperties() : this(IdentityProperty.Default.Get) {}

	readonly Func<IProperty, bool> _where;

	public ComposeIdentityProperties(Func<IProperty, bool> where) => _where = where;

	public ImmutableList<IProperty> Get(ITypeBase parameter)
	{
		var builder = ImmutableList.CreateBuilder<IProperty>();
		foreach (var property in parameter.GetProperties())
		{
			if (_where(property))
			{
				builder.Add(property);
			}
		}
		return builder.ToImmutable();
	}
}