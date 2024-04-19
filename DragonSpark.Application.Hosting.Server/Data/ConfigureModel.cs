using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;

namespace DragonSpark.Application.Hosting.Server.Data;

public class ConfigureModel : ICommand<ODataOptions>
{
	readonly IEdmModel _model;
	readonly string    _prefix;
	readonly byte      _max;

	protected ConfigureModel(IResult<IEdmModel> model, string prefix = "odata", byte max = byte.MaxValue)
		: this(model.Get(), prefix, max) {}

	protected ConfigureModel(IEdmModel model, string prefix, byte max)
	{
		_model  = model;
		_prefix = prefix;
		_max    = max;
	}

	public void Execute(ODataOptions parameter)
	{
		parameter.EnableQueryFeatures(_max).AddRouteComponents(_prefix, _model);
	}
}