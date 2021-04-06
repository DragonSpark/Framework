using DragonSpark.Application.Compose.Entities;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Entities.Design
{
	public sealed class SqlServerConfigurations<T> : ISelect<Type, Action<DbContextOptionsBuilder<T>>>
		where T : DbContext
	{
		public static SqlServerConfigurations<T> Default { get; } = new SqlServerConfigurations<T>();

		SqlServerConfigurations() {}

		public Action<DbContextOptionsBuilder<T>> Get(Type parameter)
		{
			var name   = parameter.Assembly.GetName().Name.Verify();
			var result = new ConfigureSqlServer<T>(name).ToDelegate();
			return result;
		}
	}
}