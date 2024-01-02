using Azure.Messaging.EventHubs;
using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Events.Send;

public interface ICreateEventData : ISelect<CreateEventDataInput, EventData>;