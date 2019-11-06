using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences
{
	public class Storage<T> : Select<uint, Store<T>>, IStorage<T>
	{
		readonly ICommand<T[]> _return;

		public Storage(IStores<T> stores, ICommand<T[]> @return) : base(stores.Get) => _return = @return;

		public void Execute(T[] parameter)
		{
			_return.Execute(parameter);
		}
	}
}