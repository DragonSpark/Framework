using System;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences
{
	public sealed class Enumerate<T> : IEnumerate<T>
	{
		public static Enumerate<T> Default { get; } = new Enumerate<T>();

		Enumerate() : this(Leases<Store<T>>.Default, Leases<T>.Default) {}

		readonly IStorage<T> _item;

		readonly IStorage<Store<T>> _items;
		readonly uint               _start;

		public Enumerate(IStorage<Store<T>> items, IStorage<T> item, uint start = 8_192)
		{
			_items = items;
			_item  = item;
			_start = start;
		}

		public Store<T> Get(IEnumerator<T> parameter)
		{
			var  session = _items.Get(32);
			var  items   = session.Instance;
			var  marker  = _start;
			var  total   = 0u;
			var  pages   = 0u;
			bool next;
			do
			{
				var lease  = _item.Get(Math.Min(uint.MaxValue - marker, marker *= 2));
				var store  = lease.Instance;
				var target = store.Length;
				var local  = 0u;
				while (local < target && parameter.MoveNext())
				{
					store[local++] = parameter.Current;
					total++;
				}

				items[pages++] = new Store<T>(store, local);
				next           = local == target;
			} while (next);

			var result      = _item.Get(total);
			var destination = result.Instance;
			var offset      = 0u;
			for (var i = 0u; i < pages; i++)
			{
				var item     = items[i];
				var amount   = item.Length;
				var instance = item.Instance;
				Array.Copy(instance, 0, destination, offset, amount);
				offset += amount;
				_item.Execute(instance);
			}

			_items.Execute(items);
			return result;
		}
	}
}