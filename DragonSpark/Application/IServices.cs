using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Application
{
	public interface IServices : IServiceProvider {}

	public interface IServices<in T> : ISelect<T, IServices> {}
}