using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class PropertyMethod : Instance<MethodInfo>
{
	public static PropertyMethod Default { get; } = new();

	PropertyMethod()
		: base(typeof(EF).GetMethod(nameof(EF.Property), BindingFlags.Public | BindingFlags.Static).Verify()) {}
}