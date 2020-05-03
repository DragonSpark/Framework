using System;

namespace DragonSpark.Model
{
	public readonly struct None : IEquatable<None>
	{
		public static None Default { get; } = new None();

		public bool Equals(None parameter) => parameter == Default;

		public override bool Equals(object? obj) => obj is None;

		public override int GetHashCode() => 0;

		public override string ToString() => "()";

		public static bool operator ==(None _, None __) => true;

		public static bool operator !=(None _, None __) => false;
	}
}