using DragonSpark.Model.Results;
using Flurl;

namespace DragonSpark.Application.Navigation
{
	public class BookmarkedPath : IResult<string>
	{
		readonly CurrentPath _current;
		readonly string      _property, _name;

		public BookmarkedPath(CurrentPath current, string name) : this(current, name, ScrollToSection.Default) {}

		public BookmarkedPath(CurrentPath current, string name, string property)
		{
			_current  = current;
			_name     = name;
			_property = property;
		}

		public string Get() => _current.Get().SetQueryParam(_property, _name);
	}
}