using System;

namespace DragonSpark.Objects
{
	public interface IItemProfile
	{
		Type ItemType { get; }
		string ItemName { get; }
	}
}