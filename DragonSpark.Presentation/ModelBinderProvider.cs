using DragonSpark.Compose;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DragonSpark.Presentation
{
	public sealed class ModelBinderProvider<T> : IModelBinderProvider where T : class
	{
		public static ModelBinderProvider<T> Default { get; } = new ModelBinderProvider<T>();

		ModelBinderProvider() : this(new Dictionary<Type, Type>
		{
			[A.Type<CallbackContext>()] = A.Type<CallbackContextBinder<T>>(),
			[A.Type<ProviderContext>()] = A.Type<ProviderContextBinder>()
		}) {}

		readonly IReadOnlyDictionary<Type, Type> _types;

		public ModelBinderProvider(IReadOnlyDictionary<Type, Type> types) => _types = types;

		public IModelBinder GetBinder(ModelBinderProviderContext context)
			=> _types.TryGetValue(context.Metadata.ModelType, out var result)
				   ? context.Services.GetRequiredService(result).To<IModelBinder>()
				   : null;
	}
}