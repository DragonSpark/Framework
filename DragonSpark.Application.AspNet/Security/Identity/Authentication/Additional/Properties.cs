using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

sealed class Properties : IProperties
{
    public static Properties Default { get; } = new();

    Properties() {}

    public IEnumerable<KeyValuePair<string, string?>> Get(object parameter)
    {
        foreach (var property in parameter.GetType()
                                          .GetProperties()
                                          .Where(x => Attribute.IsDefined(x, typeof(PersonalDataAttribute))))
        {
            yield return new(property.Name, property.GetValue(parameter)?.ToString());
        }
    }
}