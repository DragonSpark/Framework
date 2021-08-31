using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities.Queries
{
	public readonly record struct In<T>(DbContext Context, T Parameter);
}