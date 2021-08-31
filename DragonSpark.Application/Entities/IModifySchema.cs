using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Entities
{
	public interface IModifySchema : ISelect<Type, ICommand<ModelBuilder>?> {}

}