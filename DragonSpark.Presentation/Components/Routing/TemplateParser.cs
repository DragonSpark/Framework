using System;

// ReSharper disable All

namespace DragonSpark.Presentation.Components.Routing
{
	/// <summary>
	/// ATTRIBUTION: https://github.com/ShaunCurtis/CEC.Routing/tree/master/CEC.Routing/Routing
	/// </summary>
	class TemplateParser
	{
		public static readonly char[] InvalidParameterNameCharacters =
			new char[] {'*', '{', '}', '=', '.'};

		internal static RouteTemplate ParseTemplate(string template)
		{
			var originalTemplate = template;
			template = template.Trim('/');
			if (template == string.Empty)
			{
				// Special case "/";
				return new RouteTemplate("/", Array.Empty<TemplateSegment>());
			}

			var segments         = template.Split('/');
			var templateSegments = new TemplateSegment[segments.Length];
			for (int i = 0; i < segments.Length; i++)
			{
				var segment = segments[i];
				if (string.IsNullOrEmpty(segment))
				{
					throw new InvalidOperationException(
					                                    $"Invalid template '{template}'. Empty segments are not allowed.");
				}

				var length = segment.Length - 1;
				if (segment[0] != '{')
				{
					if (segment[length] == '}')
					{
						throw new InvalidOperationException(
						                                    $"Invalid template '{template}'. Missing '{{' in parameter segment '{segment}'.");
					}

					templateSegments[i] = new TemplateSegment(originalTemplate, segment, isParameter: false);
				}
				else
				{
					if (segment[length] != '}')
					{
						throw new InvalidOperationException(
						                                    $"Invalid template '{template}'. Missing '}}' in parameter segment '{segment}'.");
					}

					if (segment.Length < 3)
					{
						throw new InvalidOperationException(
						                                    $"Invalid template '{template}'. Empty parameter name in segment '{segment}' is not allowed.");
					}

					var count            = segment.Length - 2;
					var invalidCharacter = segment.IndexOfAny(InvalidParameterNameCharacters, 1, count);
					if (invalidCharacter != -1)
					{
						throw new InvalidOperationException(
						                                    $"Invalid template '{template}'. The character '{segment[invalidCharacter]}' in parameter segment '{segment}' is not allowed.");
					}

					templateSegments[i] =
						new TemplateSegment(originalTemplate, segment.Substring(1, count), isParameter: true);
				}
			}

			for (int i = 0; i < templateSegments.Length; i++)
			{
				var currentSegment = templateSegments[i];
				if (!currentSegment.IsParameter)
				{
					continue;
				}

				for (int j = i + 1; j < templateSegments.Length; j++)
				{
					var nextSegment = templateSegments[j];

					if (currentSegment.IsOptional && !nextSegment.IsOptional)
					{
						throw new
							InvalidOperationException($"Invalid template '{template}'. Non-optional parameters or literal routes cannot appear after optional parameters.");
					}

					if (string.Equals(currentSegment.Value, nextSegment.Value, StringComparison.OrdinalIgnoreCase))
					{
						throw new InvalidOperationException(
						                                    $"Invalid template '{template}'. The parameter '{currentSegment}' appears multiple times.");
					}
				}
			}

			return new RouteTemplate(template, templateSegments);
		}
	}
}