using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Application.Runtime
{
	public interface ITransactional<T> : ISelect<(Memory<T> Stored, Memory<T> Source), Transactions<T>> {}
}