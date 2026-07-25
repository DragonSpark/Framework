using DragonSpark.Model.Operations.Allocated;
using Microsoft.AspNetCore.Antiforgery;

namespace DragonSpark.Application.AspNet.Security.Identity;

public sealed class ValidateRequest : IAllocated
{
    readonly IAntiforgery    _antiforgery;
    readonly ICurrentContext _context;

    public ValidateRequest(IAntiforgery antiforgery, ICurrentContext context)
    {
        _antiforgery = antiforgery;
        _context     = context;
    }

    public Task Get() => _antiforgery.ValidateRequestAsync(_context.Get());
}