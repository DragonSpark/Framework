using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace DragonSpark.Application.Communication;

public interface IHeader : ISelect<IHeaderDictionary, string?>,
                           IAssign<IHeaderDictionary, string>,
                           ISelect<HttpHeaders, string?>,
                           IAssign<HttpHeaders, string>;