using Azure.Messaging.EventHubs;
using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Messaging.Events.Send;

public interface ICreateEventData : ISelect<CreateEventDataInput, EventData>;