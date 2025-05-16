using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Communication.Http;

public interface IAddClient<T> : ISelect<AddHttpClientInput<T>, IServiceCollection> where T : Options;