using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Entities.Initialization;

public interface IModifySchema : ISelect<Type, ICommand<ModelBuilder>?> {}