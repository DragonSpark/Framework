using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Alterings<T> : IAltering<T>
{
	readonly ISelecting<Many<T>, T> _alter;
	readonly IAltering<T>[]         _instances;

	public Alterings(params IAltering<T>[] instances) : this(Aggregate<T>.Default, instances) {}

	public Alterings(ISelecting<Many<T>, T> alter, params IAltering<T>[] instances)
	{
		_alter     = alter;
		_instances = instances;
	}

	public ValueTask<T> Get(T parameter) => _alter.Get(new(_instances, parameter));
}