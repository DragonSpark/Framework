using DragonSpark.Model.Results;

namespace DragonSpark.Text
{
	public class Text : Instance<string>
	{
		protected Text(string instance) : base(instance) {}

		public override string ToString() => Get();
	}
}