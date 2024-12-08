using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities;

public readonly record struct In<T>(DbContext Context, T Parameter);