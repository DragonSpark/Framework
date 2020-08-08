using DragonSpark.Model.Results;

namespace DragonSpark.Text
{
	public class Character : Instance<char>
	{
		public static implicit operator string(Character instance) => instance.ToString();

		public Character(char instance) : base(instance) {}

		public override string ToString() => Get().ToString();
	}
}
