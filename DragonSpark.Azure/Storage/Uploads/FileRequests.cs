namespace DragonSpark.Azure.Storage.Uploads;

public abstract record FileRequests(IFiles Save, IFiles Remove, IView View);