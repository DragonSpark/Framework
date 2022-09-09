using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Application.Model.Text;

public class Smart<T> : Formatter<T> where T : notnull
{
	public Smart(string template) : base(template.Start().Get().Smart) {}
}