using DragonSpark.Model;
using System;

namespace DragonSpark.Text.Formatting;

public interface IFormatEntry<T> : IPair<string, Func<T, string>> {}