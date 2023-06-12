﻿using DragonSpark.Model.Commands;
using LightInject;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

sealed class Compose : ICommand<IServiceContainer>
{
	public static Compose Default { get; } = new();

	Compose() {}

	public void Execute(IServiceContainer parameter)
	{
		parameter.RegisterScoped(typeof(IPaging<>), typeof(Paging<>))
		         .Decorate(typeof(IPaging<>), typeof(PolicyAwarePaging<>))
		         .Decorate(typeof(IPaging<>), typeof(ExceptionAwarePaging<>))
				 ;

		parameter.RegisterScoped(typeof(IAny<>), typeof(Any<>))
		         .Decorate(typeof(IAny<>), typeof(PolicyAwareAny<>))
		         .Decorate(typeof(IAny<>), typeof(ExceptionAwareAny<>))
				 ;

		parameter.RegisterScoped(typeof(IPagination<>), typeof(Pagination<>));
	}
}