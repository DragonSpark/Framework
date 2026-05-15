using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public readonly record struct LoadMembersInput(Expression Expression, EntityEntry Entry);