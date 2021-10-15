using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

public readonly record struct In<T>(DbContext Context, T Parameter);