﻿using System.Collections.Generic;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public sealed record PageResponse<T>(ICollection<T> Page, uint? Count);