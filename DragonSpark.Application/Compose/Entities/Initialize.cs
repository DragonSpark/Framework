using DragonSpark.Application.Entities;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Alterations;
using LightInject;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Compose.Entities
{
	sealed class Initialize<T> : IAlteration<BuildHostContext> where T : DbContext
	{
		public static Initialize<T> Default { get; } = new();

		Initialize() {}

		public BuildHostContext Get(BuildHostContext parameter)
			=> parameter.Decorate<T>((factory, context) => ServiceFactoryExtensions.GetInstance<IStorageInitializer<T>>(factory)
			                                                                       .Get(context));
	}
}