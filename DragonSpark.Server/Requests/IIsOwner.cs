using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Server.Requests;

public interface IIsOwner : IStopAware<Unique, bool?>;