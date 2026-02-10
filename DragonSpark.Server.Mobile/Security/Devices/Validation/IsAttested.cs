using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

namespace DragonSpark.Server.Mobile.Security.Devices.Validation;

sealed class IsAttested : EvaluateToAny<string, ValidationRecordBase>, IIsAttested
{
    public IsAttested(IScopes scopes) : base(scopes, SelectValidatedDevice.Default) {}
}