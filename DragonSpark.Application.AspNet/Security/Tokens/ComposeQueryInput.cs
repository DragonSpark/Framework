using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Security.Tokens;

public readonly record struct ComposeQueryInput(DbSet<Nonce> Source, string Identity, NoncePurpose? Type);