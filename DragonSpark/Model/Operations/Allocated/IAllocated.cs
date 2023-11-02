﻿using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public interface IAllocated : IResult<Task>;

public interface IAllocated<in T> : ISelect<T, Task>;