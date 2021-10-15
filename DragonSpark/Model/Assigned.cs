namespace DragonSpark.Model;

public readonly struct Assigned<T> where T : struct
{
	public static Assigned<T> Unassigned { get; } = new Assigned<T>(default, false);

	public static implicit operator Assigned<T>(in T value) => new Assigned<T>(value);

	public static implicit operator T(in Assigned<T> value) => value.Instance;

	public Assigned(in T instance, bool assigned = true)
	{
		Instance   = instance;
		IsAssigned = assigned;
	}

	public T Instance { get; }

	public bool IsAssigned { get; }

	public override int GetHashCode() => Instance.GetHashCode();

	public override string? ToString() => IsAssigned ? Instance.ToString() : string.Empty;

	bool Equals(Assigned<T> other) => Instance.Equals(other.Instance);

	public override bool Equals(object? obj) => obj is Assigned<T> other && Equals(other);

	public static bool operator ==(Assigned<T> left, Assigned<T> right) => left.Equals(right);

	public static bool operator !=(Assigned<T> left, Assigned<T> right) => !left.Equals(right);
}