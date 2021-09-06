using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Members;
using System;
using System.Reflection;

namespace DragonSpark.Presentation.Components.Scoped
{
	sealed class Properties : ArrayStore<Type, PropertyInfo>
	{
		public static Properties Default { get; } = new Properties();

		Properties() : base(Properties<ScopedInjectionAttribute>.Default) {}
	}
}