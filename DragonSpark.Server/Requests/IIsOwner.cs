using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Server.Requests;

public interface IIsOwner : ISelecting<Unique, bool?> {}