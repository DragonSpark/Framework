using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

public readonly record struct Scope(DbContext Subject, IBoundary Boundary);