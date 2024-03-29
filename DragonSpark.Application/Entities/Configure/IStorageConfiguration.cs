﻿using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Entities.Configure;

public interface IStorageConfiguration : ISelect<IServiceCollection, Action<DbContextOptionsBuilder>>;