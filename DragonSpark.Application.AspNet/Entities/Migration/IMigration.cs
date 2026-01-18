using DragonSpark.Model.Commands;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public interface IMigration : ICommand<ushort>;