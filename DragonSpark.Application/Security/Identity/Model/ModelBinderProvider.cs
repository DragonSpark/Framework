using DragonSpark.Compose;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Security.Identity.Model
{
	public sealed class ModelBinderProvider : IModelBinderProvider
	{
		public static ModelBinderProvider Default { get; } = new ModelBinderProvider();

		ModelBinderProvider() : this(new Dictionary<Type, Type>
		{
			[A.Type<CallbackContext>()] = A.Type<CallbackContextBinder>(),
			[A.Type<ProviderContext>()] = A.Type<ProviderContextBinder>()
		}) {}

		readonly IReadOnlyDictionary<Type, Type> _types;

		public ModelBinderProvider(IReadOnlyDictionary<Type, Type> types) => _types = types;

		public IModelBinder GetBinder(ModelBinderProviderContext context)
			=> _types.TryGetValue(context.Metadata.ModelType, out var result)
				   ? context.Services.GetRequiredService(result).To<IModelBinder>()
				   : null!;
	}
}