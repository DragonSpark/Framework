using Microsoft.AspNetCore.Components;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Presentation.Components.Routing;

/// <summary>
/// ATTRIBUTION: https://github.com/ShaunCurtis/CEC.Routing/tree/master/CEC.Routing/Routing
/// </summary>
static class RouteTableFactory
{
	private readonly static ConcurrentDictionary<Key, RouteTable> Cache = new();
	readonly static         IComparer<RouteEntry> RoutePrecedence = Comparer<RouteEntry>.Create(RouteComparison);

	public static RouteTable Create(IEnumerable<Assembly> assemblies)
	{
		var key = new Key(assemblies.OrderBy(a => a.FullName).ToArray());
		if (Cache.TryGetValue(key, out var resolvedComponents))
		{
			return resolvedComponents;
		}

		var componentTypes =
			key.Assemblies.SelectMany(a => a.ExportedTypes.Where(t => typeof(IComponent).IsAssignableFrom(t)));
		var routeTable = Create(componentTypes);
		Cache.TryAdd(key, routeTable);
		return routeTable;
	}

	static RouteTable Create(IEnumerable<Type> componentTypes)
	{
		var templatesByHandler = new Dictionary<Type, string[]>();
		foreach (var componentType in componentTypes)
		{
			// We're deliberately using inherit = false here.
			//
			// RouteAttribute is defined as non-inherited, because inheriting a route attribute always causes an
			// ambiguity. You end up with two components (base class and derived class) with the same route.
			var routeAttributes = componentType.GetCustomAttributes<RouteAttribute>(inherit: false);

			var templates = routeAttributes.Select(t => t.Template).ToArray();
			templatesByHandler.Add(componentType, templates);
		}

		return Create(templatesByHandler);
	}

	static RouteTable Create(Dictionary<Type, string[]> templatesByHandler)
	{
		var comparer = StringComparer.OrdinalIgnoreCase;
		var routes   = new List<RouteEntry>();
		foreach (var (key, value) in templatesByHandler)
		{
			var parsedTemplates = value.Select(TemplateParser.ParseTemplate).ToArray();
			using var all = parsedTemplates.SelectMany(GetParameterNames)
			                               .AsValueEnumerable()
			                               .Distinct(comparer)
			                               .ToArray(ArrayPool<string>.Shared);

			foreach (var parsedTemplate in parsedTemplates)
			{
				var unused = all.Except(GetParameterNames(parsedTemplate), comparer).ToArray();
				routes.Add(new(parsedTemplate, key, unused));
			}
		}

		return new(routes.OrderBy(id => id, RoutePrecedence).ToArray());
	}

	private static IEnumerable<string> GetParameterNames(RouteTemplate routeTemplate)
		=> routeTemplate.Segments.Where(s => s.IsParameter).Select(s => s.Value);

	/// <summary>
	/// Route precedence algorithm.
	/// We collect all the routes and sort them from most specific to
	/// less specific. The specificity of a route is given by the specificity
	/// of its segments and the position of those segments in the route.
	/// * A literal segment is more specific than a parameter segment.
	/// * A parameter segment with more constraints is more specific than one with fewer constraints
	/// * Segment earlier in the route are evaluated before segments later in the route.
	/// For example:
	/// /Literal is more specific than /Parameter
	/// /Route/With/{parameter} is more specific than /{multiple}/With/{parameters}
	/// /Product/{id:int} is more specific than /Product/{id}
	///
	/// Routes can be ambiguous if:
	/// They are composed of literals and those literals have the same values (case insensitive)
	/// They are composed of a mix of literals and parameters, in the same relative order and the
	/// literals have the same values.
	/// For example:
	/// * /literal and /Literal
	/// /{parameter}/literal and /{something}/literal
	/// /{parameter:constraint}/literal and /{something:constraint}/literal
	///
	/// To calculate the precedence we sort the list of routes as follows:
	/// * Shorter routes go first.
	/// * A literal wins over a parameter in precedence.
	/// * For literals with different values (case insensitive) we choose the lexical order
	/// * For parameters with different numbers of constraints, the one with more wins
	/// If we get to the end of the comparison routing we've detected an ambiguous pair of routes.
	/// </summary>
	// ReSharper disable once MethodTooLong
	// ReSharper disable once ExcessiveIndentation
	// ReSharper disable once CognitiveComplexity
	internal static int RouteComparison(RouteEntry x, RouteEntry y)
	{
		if (ReferenceEquals(x, y))
		{
			return 0;
		}

		var xTemplate = x.Template;
		var yTemplate = y.Template;
		if (xTemplate.Segments.Length != y.Template.Segments.Length)
		{
			return xTemplate.Segments.Length < y.Template.Segments.Length ? -1 : 1;
		}
		else
		{
			for (var i = 0; i < xTemplate.Segments.Length; i++)
			{
				var xSegment = xTemplate.Segments[i];
				var ySegment = yTemplate.Segments[i];
				if (!xSegment.IsParameter && ySegment.IsParameter)
				{
					return -1;
				}

				if (xSegment.IsParameter && !ySegment.IsParameter)
				{
					return 1;
				}

				if (xSegment.IsParameter)
				{
					// Always favor non-optional parameters over optional ones
					if (!xSegment.IsOptional && ySegment.IsOptional)
					{
						return -1;
					}

					if (xSegment.IsOptional && !ySegment.IsOptional)
					{
						return 1;
					}

					if (xSegment.Constraints.Length > ySegment.Constraints.Length)
					{
						return -1;
					}
					else if (xSegment.Constraints.Length < ySegment.Constraints.Length)
					{
						return 1;
					}
				}
				else
				{
					var comparison =
						string.Compare(xSegment.Value, ySegment.Value, StringComparison.OrdinalIgnoreCase);
					if (comparison != 0)
					{
						return comparison;
					}
				}
			}

			throw new InvalidOperationException($@"The following routes are ambiguous:
'{x.Template.TemplateText}' in '{x.Handler.FullName}'
'{y.Template.TemplateText}' in '{y.Handler.FullName}'
");
		}
	}

	private readonly struct Key : IEquatable<Key>
	{
		public readonly Assembly[] Assemblies;

		public Key(Assembly[] assemblies)
		{
			Assemblies = assemblies;
		}

		public override bool Equals(object? obj) => obj is Key other && base.Equals(other);

		public bool Equals(Key other)
		{
			// ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
			if (Assemblies == null && other.Assemblies == null)
			{
				return true;
			}
			// ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
			else if (Assemblies == null ^ other.Assemblies == null)
			{
				return false;
			}
			else if (Assemblies?.Length != other.Assemblies?.Length)
			{
				return false;
			}

			for (var i = 0; i < Assemblies?.Length; i++)
			{
				if (!Assemblies[i].Equals(other.Assemblies![i]))
				{
					return false;
				}
			}

			return true;
		}

		public override int GetHashCode()
		{
			var hash = new HashCodeCombiner();

			if (Assemblies != null)
			{
				for (var i = 0; i < Assemblies.Length; i++)
				{
					hash.Add(Assemblies[i]);
				}
			}

			return hash;
		}
	}
}