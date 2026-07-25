namespace DragonSpark.Application.AspNet.Security.Tokens;

public readonly record struct ComposeQueryResult(IQueryable<Nonce> Query, DateTime Now);