using System;

namespace DragonSpark.Model.Sequences;

public readonly struct Selection : IEquatable<Selection>
{
	public static Selection Default { get; } = new(0);

	public static implicit operator Selection(Assigned<uint> length) => new(0, length);

	public Selection(uint start, Assigned<uint> length = default)
	{
		Start  = start;
		Length = length;
	}

	public uint Start { get; }

	public Assigned<uint> Length { get; }

	public static bool operator ==(Selection left, Selection right) => left.Equals(right);

	public static bool operator !=(Selection left, Selection right) => !left.Equals(right);

	public bool Equals(Selection other) => Start == other.Start && Length == other.Length;

	public override bool Equals(object? obj) => obj is Selection other && Equals(other);

	public override int GetHashCode()
	{
		unchecked
		{
			return ((int)Start * 397) ^ Length.GetHashCode();
		}
	}
}