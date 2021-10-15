using DragonSpark.Model.Selection.Stores;
using System.Reflection;

namespace DragonSpark.Reflection;

public interface IMetadataTable<TMetadata, TValue> : ITable<TMetadata, TValue> where TMetadata : MemberInfo {}