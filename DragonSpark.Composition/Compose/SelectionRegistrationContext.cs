using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime.Activation;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Compose
{
	public sealed class SelectionRegistrationContext<T> : IRegistrationContext where T : class
	{
		readonly IServiceCollection        _collection;
		readonly Func<IServiceProvider, T> _select;

		public SelectionRegistrationContext(IServiceCollection collection, Func<IServiceProvider, T> select)
		{
			_collection = collection;
			_select     = select;
		}

		public IServiceCollection Singleton() => _collection.AddSingleton(_select);

		public IServiceCollection Transient() => _collection.AddTransient(_select);

		public IServiceCollection Scoped() => _collection.AddScoped(_select);
	}

	sealed class GenericTypeDependencySelector : ValidatedAlteration<Type>, IActivateUsing<Type>
	{
		public GenericTypeDependencySelector(Type type)
			: base(Start.A.Selection.Of.System.Type.By.Returning(IsGenericTypeDefinition.Default.Get(type)),
			       GenericTypeDefinition.Default.Then()
			                            .Ensure.Input.Is(IsDefinedGenericType.Default)
			                            .Otherwise.Use(x => x)) {}
	}
}