using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Diagnostics;
using DragonSpark.Application.Mobile.Model.Validation;
using DragonSpark.Compose;
using Metalama.Extensions.DependencyInjection;
using Metalama.Framework.Aspects;
using Refit;

namespace DragonSpark.Application.Mobile.Aspects;

public sealed class ExceptionAwareAttribute : OverrideMethodAspect
{
    [IntroduceDependency(IsRequired = true)]
    readonly ILastChanceExceptionHandler? _exceptionHandler = null;

    public ExceptionAwareAttribute() => UseAsyncTemplateForAnyAwaitable = true;

    // ReSharper disable once MethodTooLong
    // ReSharper disable once CognitiveComplexity
    // ReSharper disable once CyclomaticComplexity
    public override async Task<dynamic?> OverrideAsyncMethod()
    {
        var aware = meta.This as IValidationAware;
        aware?.Get().External.Clear();

        try
        {
            return await meta.ProceedAsync().On(); 
        }
        catch (ValidationApiException e) when(e.Content is not null)
        {
            if (aware is not null && e.Content.Errors.Count > 0)
            {
                var model = aware.Get();
                foreach (var (key, value) in e.Content.Errors)
                {
                    model.External.Add(key, value);
                }

                aware.Execute(new (model.Local, model.External));
            }
        }
        catch (ApiException e) when (e.StatusCode == HttpStatusCode.BadRequest)
        {
            var details = e.HasContent ? JsonSerializer.Deserialize<Dictionary<string, string[]>>(e.Content) : null;
            if (aware is not null && details is not null)
            {
                var model = aware.Get();
                foreach (var (key, value) in details)
                {
                    model.External.Add(key, value);
                }

                aware.Execute(new (model.Local, model.External));
            }
        }
        catch (Exception e) when (_exceptionHandler?.Condition.Get(e) ?? false)
        {
            var parameter = meta.Target.Parameters.LastOrDefault(p => p.Type.Equals(typeof(CancellationToken)));
            var token     = parameter is not null ? parameter.Value : CancellationToken.None;
            await _exceptionHandler.Off(new(e, token));
        }
        return null;
    }
    
    /*
    void Update(object instance, Dictionary<string, string[]> parameter)
    {
        if (instance is IValidationAware aware && parameter.Count > 0)
        {
            var model = aware.Get();
            foreach (var (key, value) in parameter)
            {
                model.External.Add(key, value);
            }

            aware.Execute(new (model.Local, model.External));
        }
    }
    */

    public override dynamic? OverrideMethod()
    {
        try
        {
            return meta.Proceed();
        }
        catch (Exception e) when (_exceptionHandler?.Condition.Get(e) ?? false)
        {
            _exceptionHandler.Get(new(e, CancellationToken.None)).AsTask().GetAwaiter().GetResult();
            return null;
        }
    }
}