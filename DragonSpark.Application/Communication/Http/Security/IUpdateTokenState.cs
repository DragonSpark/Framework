using DragonSpark.Contracts.Security;
using DragonSpark.Model.Operations.Selection.Stop.Conditions;

namespace DragonSpark.Application.Communication.Http.Security;

public interface IUpdateTokenState : IDepending<AccessTokenView?>;