using System;

namespace DragonSpark.IoC.Commands
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class ConfiguresApplicationAttribute : Attribute
	{}
}
