using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation
{
	public interface IViewProperty : IOperation {}

	public sealed class ViewProperty<T> : IViewProperty
	{
		public static implicit operator ViewProperty<T>(ValueTask<T> instance) => new ViewProperty<T>(instance);

		readonly ValueTask<T> _source;

		public ViewProperty(ValueTask<T> source) => _source = source;

		public T Value { get; private set; }

		public bool HasValue { get; private set; }

		public async ValueTask Get()
		{
			Value = await _source;
			HasValue = true;
		}

		public override string ToString() => Value.ToString();
	}
}