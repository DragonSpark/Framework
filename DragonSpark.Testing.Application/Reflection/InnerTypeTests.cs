﻿using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Reflection.Collections;
using FluentAssertions;
using System.Reflection;
using Xunit;

namespace DragonSpark.Testing.Application.Reflection
{
	public class InnerTypeTests
	{
		[Fact]
		public void Coverage()
		{
			InnerType.Default.Get(GetType())
			         .Should()
			         .BeNull();
		}

		[Fact]
		void Verify()
		{
			InnerType.Default.Get(A.Type<Always<TypeInfo>>()).Should().Be<TypeInfo>();
		}
	}
}