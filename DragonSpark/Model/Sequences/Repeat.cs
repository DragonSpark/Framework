namespace DragonSpark.Model.Sequences;

sealed class Repeat<T> : IArray<uint, T>
{
	readonly Func<T> _create;

	public Repeat(Func<T> create) => _create = create;

	public Array<T> Get(uint parameter)
	{
		var result = new T[parameter];
		for (var i = 0; i < parameter; i++)
		{
			result[i] = _create();
		}

		return result;
	}
}