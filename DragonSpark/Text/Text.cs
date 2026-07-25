using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Text;

public class Text : Result<string>, IText
{
	protected Text(string instance) : base(instance.Self) {}

    public Text(IResult<string> result) : base(result) {}

    public Text(Func<string> source) : base(source) {}

    public override string ToString() => Get();
}