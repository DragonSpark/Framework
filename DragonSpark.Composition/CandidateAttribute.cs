using System;

namespace DragonSpark.Composition;

[AttributeUsage(AttributeTargets.Constructor)]
public sealed class CandidateAttribute : Attribute
{
	public CandidateAttribute(bool enabled = true) => Enabled = enabled;

	public bool Enabled { get; }
}