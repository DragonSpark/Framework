using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Messaging;

sealed class AllowSend : FixedResultCondition<AllowSendInput>, IAllowSend
{
	public AllowSend(EmailMessagingSettings settings) : base(settings.Enabled) {}
}