using System;
using System.Collections.Concurrent;
using System.Globalization;
// ReSharper disable All

namespace DragonSpark.Presentation.Components.Routing
{
	/// <summary>
	/// ATTRIBUTION: https://github.com/ShaunCurtis/CEC.Routing/tree/master/CEC.Routing/Routing
	/// </summary>
	abstract class RouteConstraint
	{
		// note: the things that prevent this cache from growing unbounded is that
		// we're the only caller to this code path, and the fact that there are only
		// 8 possible instances that we create.
		//
		// The values passed in here for parsing are always static text defined in route attributes.
		private static readonly ConcurrentDictionary<string, RouteConstraint> _cachedConstraints
			= new ConcurrentDictionary<string, RouteConstraint>();

		public abstract bool Match(string pathSegment, out object convertedValue);

		public static RouteConstraint Parse(string template, string segment, string constraint)
		{
			if (string.IsNullOrEmpty(constraint))
			{
				throw new
					ArgumentException($"Malformed segment '{segment}' in route '{template}' contains an empty constraint.");
			}

			if (_cachedConstraints.TryGetValue(constraint, out var cachedInstance))
			{
				return cachedInstance;
			}
			else
			{
				var newInstance = CreateRouteConstraint(constraint);
				if (newInstance != null)
				{
					// We've done to the work to create the constraint now, but it's possible
					// we're competing with another thread. GetOrAdd can ensure only a single
					// instance is returned so that any extra ones can be GC'ed.
					return _cachedConstraints.GetOrAdd(constraint, newInstance);
				}
				else
				{
					throw new ArgumentException($"Unsupported constraint '{constraint}' in route '{template}'.");
				}
			}
		}

		/// <summary>
		/// Creates a structured RouteConstraint object given a string that contains
		/// the route constraint. A constraint is the place after the colon in a
		/// parameter definition, for example `{age:int?}`.
		///
		/// If the constraint denotes an optional, this method will return an
		/// <see cref="OptionalTypeRouteConstraint{T}" /> which handles the appropriate checks.
		/// </summary>
		/// <param name="constraint">String representation of the constraint</param>
		/// <returns>Type-specific RouteConstraint object</returns>
		private static RouteConstraint CreateRouteConstraint(string constraint)
		{
			return constraint switch
			{
				"bool" => new TypeRouteConstraint<bool>(bool.TryParse),
				"bool?" => new OptionalTypeRouteConstraint<bool>(bool.TryParse),
				"datetime" => new TypeRouteConstraint<DateTime>((string str, out DateTime result)
					                                                => DateTime.TryParse(str,
					                                                                     CultureInfo.InvariantCulture,
					                                                                     DateTimeStyles.None,
					                                                                     out result)),
				"datetime?" => new OptionalTypeRouteConstraint<DateTime>((string str, out DateTime result)
					                                                         => DateTime.TryParse(str,
					                                                                              CultureInfo
						                                                                              .InvariantCulture,
					                                                                              DateTimeStyles.None,
					                                                                              out result)),
				"decimal" => new TypeRouteConstraint<decimal>((string str, out decimal result)
					                                              => decimal.TryParse(str, NumberStyles.Number,
					                                                                  CultureInfo.InvariantCulture,
					                                                                  out result)),
				"decimal?" => new OptionalTypeRouteConstraint<decimal>((string str, out decimal result)
					                                                       => decimal.TryParse(str, NumberStyles.Number,
					                                                                           CultureInfo
						                                                                           .InvariantCulture,
					                                                                           out result)),
				"double" => new TypeRouteConstraint<double>((string str, out double result)
					                                            => double.TryParse(str, NumberStyles.Number,
					                                                               CultureInfo.InvariantCulture,
					                                                               out result)),
				"double?" => new OptionalTypeRouteConstraint<double>((string str, out double result)
					                                                     => double.TryParse(str, NumberStyles.Number,
					                                                                        CultureInfo
						                                                                        .InvariantCulture,
					                                                                        out result)),
				"float" => new TypeRouteConstraint<float>((string str, out float result)
					                                          => float.TryParse(str, NumberStyles.Number,
					                                                            CultureInfo.InvariantCulture,
					                                                            out result)),
				"float?" => new OptionalTypeRouteConstraint<float>((string str, out float result)
					                                                   => float.TryParse(str, NumberStyles.Number,
					                                                                     CultureInfo.InvariantCulture,
					                                                                     out result)),
				"guid" => new TypeRouteConstraint<Guid>(Guid.TryParse),
				"guid?" => new OptionalTypeRouteConstraint<Guid>(Guid.TryParse),
				"int" => new TypeRouteConstraint<int>((string str, out int result)
					                                      => int.TryParse(str, NumberStyles.Integer,
					                                                      CultureInfo.InvariantCulture, out result)),
				"int?" => new OptionalTypeRouteConstraint<int>((string str, out int result)
					                                               => int.TryParse(str, NumberStyles.Integer,
					                                                               CultureInfo.InvariantCulture,
					                                                               out result)),
				"long" => new TypeRouteConstraint<long>((string str, out long result)
					                                        => long.TryParse(str, NumberStyles.Integer,
					                                                         CultureInfo.InvariantCulture, out result)),
				"long?" => new OptionalTypeRouteConstraint<long>((string str, out long result)
					                                                 => long.TryParse(str, NumberStyles.Integer,
					                                                                  CultureInfo.InvariantCulture,
					                                                                  out result)),
				_ => null!
			};
		}
	}
}