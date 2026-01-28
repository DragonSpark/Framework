using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.AspNet.Security.Tokens;

public interface IIssueNonce : ISelecting<IssueNonceInput, string>;