using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation
{
	public sealed class ViewProperty<T> : IOperation
	{
		public static implicit operator ViewProperty<T>(ValueTask<T> instance) => new ViewProperty<T>(instance);

		readonly ValueTask<T> _source;

		public ViewProperty(ValueTask<T> source) => _source = source;

		public T Value { get; private set; }

		public async ValueTask Get()
		{
			Value = await _source;
		}
	} }