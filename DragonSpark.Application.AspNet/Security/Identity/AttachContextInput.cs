using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Security.Identity;

public readonly record struct AttachContextInput<T>(UserManager<T> Manager, DbContext Existing) where T : class;