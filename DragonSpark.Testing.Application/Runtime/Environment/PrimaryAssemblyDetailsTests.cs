﻿using System.Reflection;
using FluentAssertions;
using DragonSpark.Reflection.Assemblies;
using DragonSpark.Runtime.Environment;
using Xunit;

namespace DragonSpark.Testing.Application.Runtime.Environment
{
	public sealed class PrimaryAssemblyDetailsTests
	{
		[Fact]
		void Verify()
		{
			var details  = PrimaryAssemblyDetails.Default.Get();
			var assembly = Assembly.GetExecutingAssembly();
			details.Version.Should().BeEquivalentTo(assembly.GetName().Version);
			details.Title.Should().NotBeNull().And.Be(AssemblyTitle.Default.Get(assembly));
		}
	}
}