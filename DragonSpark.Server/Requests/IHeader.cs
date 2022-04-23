using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Requests;

public interface IHeader : ISelect<IHeaderDictionary, string?> {}