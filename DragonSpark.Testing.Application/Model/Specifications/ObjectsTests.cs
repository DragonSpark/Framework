using DragonSpark.Model.Selection.Conditions;
using FluentAssertions;
using System;
using System.Reflection;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Specifications
{
	public class ObjectsTests
	{
		[Fact]
		public void Coverage()
		{
			var typeInfo = GetType().GetTypeInfo();
			Always<Type>.Default.Get(typeInfo).Should().BeTrue();
		}
	}
}