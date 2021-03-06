﻿using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Specifications
{
	public class InverseConditionTests
	{
		[Fact]
		public void Verify()
		{
			Always<object>.Default.Then()
			              .Inverse()
			              .Get()
			              .Get(new object())
			              .Should()
			              .BeFalse();
		}
	}
}