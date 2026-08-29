using DragonSpark.Model.Commands;
using Microsoft.Extensions.Configuration;

namespace DragonSpark.Application.Configuration;

public interface IAssign : ICommand<IConfiguration>;