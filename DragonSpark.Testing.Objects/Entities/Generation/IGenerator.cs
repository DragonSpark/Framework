﻿using AutoBogus;
using DragonSpark.Model.Selection;

namespace DragonSpark.Testing.Objects.Entities.Generation;

public interface IGenerator<T> : ISelect<Configuration, AutoFaker<T>> where T : class;