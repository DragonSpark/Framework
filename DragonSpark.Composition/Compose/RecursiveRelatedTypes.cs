﻿using DragonSpark.Model.Sequences.Memory;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;

namespace DragonSpark.Composition.Compose
{
	sealed class RecursiveRelatedTypes : IRelatedTypes
	{
		readonly IRelatedTypes _related;
		readonly ILeases<Type> _leases;

		public RecursiveRelatedTypes(IRelatedTypes related) : this(related, Leases<Type>.Default) {}

		public RecursiveRelatedTypes(IRelatedTypes related, ILeases<Type> leases)
		{
			_related = related;
			_leases  = leases;
		}

		void Expand(ISet<Type> state, Type current)
		{
			using var related = _related.Get(current);
			var       length  = related.Length;
			for (int i = 0; i < length; i++)
			{
				var item = related[i];
				if (state.Add(item))
				{
					Expand(state, item);
				}
			}
		}

		public Lease<Type> Get(Type parameter)
		{
			var state = new HashSet<Type>();
			Expand(state, parameter);
			var count = (uint)state.Count;
			var lease = _leases.Get(count);
			var index = 0;
			foreach (var type in state)
			{
				lease[index++] = type;
			}
			var result = lease.Distinct();
			return result;
		}
	}
}