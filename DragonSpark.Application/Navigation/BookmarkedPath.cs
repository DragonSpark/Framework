using DragonSpark.Model.Results;
using Flurl;

namespace DragonSpark.Application.Navigation;

public class BookmarkedPath : IResult<string>
{
	public static implicit operator string(BookmarkedPath instance) => instance.ToString();

	readonly IResult<string> _current;
	readonly string          _property, _name;

	protected BookmarkedPath(IResult<string> current, string name) : this(current, name, ScrollToSection.Default) {}

	protected BookmarkedPath(IResult<string> current, string name, string property)
	{
		_current  = current;
		_name     = name;
		_property = property;
	}

	public string Get() => _current.Get().SetQueryParam(_property, _name);

	public override string ToString() => Get();
}