using System;

namespace DragonSpark.Model.Sequences.Query
{
	readonly ref struct DynamicStore<T>
	{
		readonly static Leases<T>        Item  = Leases<T>.Default;
		readonly static Leases<Store<T>> Items = Leases<Store<T>>.Default;

		readonly Store<T>[] _stores;
		readonly Selection  _position;
		readonly uint       _index;

		public DynamicStore(uint size, uint length = 32) : this(Items.Get(length).Instance, Selection.Default)
			=> _stores[0] = new Store<T>(Item.Get(size).Instance, 0);

		DynamicStore(Store<T>[] stores, in Selection position, uint index = 0)
		{
			_stores   = stores;
			_position = position;
			_index    = index;
		}

		public Store<T> Get() => Get(DefaultStorage<T>.Default);

		public Store<T> Get(IStores<T> storage)
		{
			var result   = storage.Get(_position.Start + _position.Length);
			var instance = result.Instance;
			using (new Session<Store<T>>(_stores, Items))
			{
				var total = _index + 1;
				for (uint i = 0u, offset = 0u; i < total; i++)
				{
					var store = _stores[i];
					using (new Session<T>(store, Item))
					{
						store.Instance.CopyInto(instance, new Selection(0, store.Length), offset);
					}

					offset += store.Length;
				}
			}

			return result;
		}

		public DynamicStore<T> Add(in Store<T> page)
		{
			var current  = _stores[_index];
			var capacity = (uint)current.Instance.Length;
			var filled   = page.Length;
			var size     = filled + current.Length;

			if (size > capacity)
			{
				_stores[_index] =
					new Store<T>(page.Instance.CopyInto(current.Instance, 0, capacity - current.Length, current.Length),
					             capacity);
				var remainder = size - capacity;
				var next      = capacity * 2;
				_stores[_index + 1] =
					new Store<T>(page.Instance
					                 .CopyInto(Item.Get(Math.Max(remainder * 2, Math.Min(int.MaxValue - next, next)))
					                               .Instance,
					                           capacity - current.Length, remainder),
					             remainder);

				return new DynamicStore<T>(_stores, new Selection(_position.Start + capacity, remainder), _index + 1);
			}

			_stores[_index]
				= new Store<T>(page.Instance.CopyInto(current.Instance, 0, filled, current.Length),
				               current.Length + filled);
			return new DynamicStore<T>(_stores, new Selection(_position.Start, _position.Length + filled), _index);
		}
	}
}