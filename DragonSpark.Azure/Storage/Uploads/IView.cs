using DragonSpark.Server.Requests;

namespace DragonSpark.Azure.Storage.Uploads;

public interface IView : IInput<ViewInput, IStorageEntry?>;