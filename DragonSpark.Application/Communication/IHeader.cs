using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.Communication;

public interface IHeader : ISelect<IHeaderDictionary, string?> {}