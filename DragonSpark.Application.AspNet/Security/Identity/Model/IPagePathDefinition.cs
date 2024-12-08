﻿using DragonSpark.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public interface IPagePathDefinition : IFormatter<(IUrlHelper Url, IValueProvider Values)>;