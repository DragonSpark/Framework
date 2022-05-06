namespace DragonSpark.Application.Runtime;

public interface ILargeOrderAware
{
	public uint? Order { get; set; }
}

// TODO

public interface IMediumOrderAware
{
	public ushort? Order { get; set; }
}

public interface IOrderAware
{
	public byte Order { get; set; }
}
