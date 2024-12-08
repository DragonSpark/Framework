using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public interface IInitialize : IOperation<DbContext>;