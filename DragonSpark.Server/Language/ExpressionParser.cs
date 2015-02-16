using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Server.Legacy.Language
{
	internal class ExpressionParser
	{
		private struct Token
		{
			#region Fields
			public TokenId id;

			public int pos;

			public string text;
			#endregion
		}

		private enum TokenId
		{
			Unknown,
			End,
			Identifier,
			StringLiteral,
			IntegerLiteral,
			RealLiteral,
			Exclamation,
			Percent,
			Amphersand,
			OpenParen,
			CloseParen,
			Asterisk,
			Plus,
			Comma,
			Minus,
			Dot,
			Slash,
			Colon,
			LessThan,
			Equal,
			GreaterThan,
			Question,
			OpenBracket,
			CloseBracket,
			Bar,
			ExclamationEqual,
			DoubleAmphersand,
			LessThanEqual,
			LessGreater,
			DoubleEqual,
			GreaterThanEqual,
			DoubleBar
		}

		private interface ILogicalSignatures
		{
			void F(bool x, bool y);

			void F(bool? x, bool? y);
		}

		private interface IArithmeticSignatures
		{
			void F(int x, int y);

			void F(uint x, uint y);

			void F(long x, long y);

			void F(ulong x, ulong y);

			void F(float x, float y);

			void F(double x, double y);

			void F(decimal x, decimal y);

			void F(int? x, int? y);

			void F(uint? x, uint? y);

			void F(long? x, long? y);

			void F(ulong? x, ulong? y);

			void F(float? x, float? y);

			void F(double? x, double? y);

			void F(decimal? x, decimal? y);
		}

		private interface IRelationalSignatures : IArithmeticSignatures
		{
			void F(string x, string y);

			void F(char x, char y);

			void F(DateTime x, DateTime y);

			void F(TimeSpan x, TimeSpan y);

			void F(char? x, char? y);

			void F(DateTime? x, DateTime? y);

			void F(TimeSpan? x, TimeSpan? y);
		}

		private interface IEqualitySignatures : IRelationalSignatures
		{
			void F(bool x, bool y);

			void F(bool? x, bool? y);
		}

		private interface IAddSignatures : IArithmeticSignatures
		{
			void F(DateTime x, TimeSpan y);

			void F(TimeSpan x, TimeSpan y);

			void F(DateTime? x, TimeSpan? y);

			void F(TimeSpan? x, TimeSpan? y);
		}

		private interface ISubtractSignatures : IAddSignatures
		{
			void F(DateTime x, DateTime y);

			void F(DateTime? x, DateTime? y);
		}

		private interface INegationSignatures
		{
			void F(int x);

			void F(long x);

			void F(float x);

			void F(double x);

			void F(decimal x);

			void F(int? x);

			void F(long? x);

			void F(float? x);

			void F(double? x);

			void F(decimal? x);
		}

		private interface INotSignatures
		{
			void F(bool x);

			void F(bool? x);
		}

		private interface IEnumerableSignatures
		{
			void Where(bool predicate);

			void Any();

			void Any(bool predicate);

			void All(bool predicate);

			void Count();

			void Count(bool predicate);

			void Min(object selector);

			void Max(object selector);

			void Sum(int selector);

			void Sum(int? selector);

			void Sum(long selector);

			void Sum(long? selector);

			void Sum(float selector);

			void Sum(float? selector);

			void Sum(double selector);

			void Sum(double? selector);

			void Sum(decimal selector);

			void Sum(decimal? selector);

			void Average(int selector);

			void Average(int? selector);

			void Average(long selector);

			void Average(long? selector);

			void Average(float selector);

			void Average(float? selector);

			void Average(double selector);

			void Average(double? selector);

			void Average(decimal selector);

			void Average(decimal? selector);
		}

		private static readonly Type[] predefinedTypes = {
			typeof(Object), typeof(Boolean), typeof(Char), typeof(String), typeof(SByte), typeof(Byte), typeof(Int16), typeof(UInt16), typeof(Int32), typeof(UInt32), typeof(Int64), typeof(UInt64), typeof(Single), typeof(Double), typeof(Decimal),
			typeof(DateTime), typeof(TimeSpan), typeof(Guid), typeof(Math), typeof(Convert)
		};

		private static readonly System.Linq.Expressions.Expression trueLiteral = System.Linq.Expressions.Expression.Constant(true);

		private static readonly System.Linq.Expressions.Expression falseLiteral = System.Linq.Expressions.Expression.Constant(false);

		private static readonly System.Linq.Expressions.Expression nullLiteral = System.Linq.Expressions.Expression.Constant(null);

		private static readonly string keywordIt = "it";

		private static readonly string keywordIif = "iif";

		private static readonly string keywordNew = "new";

		private static Dictionary<string, object> keywords;

		private readonly Dictionary<string, object> symbols;

		private IDictionary<string, object> externals;

		private readonly Dictionary<System.Linq.Expressions.Expression, string> literals;

		private ParameterExpression it;

		private readonly string textField;

		private int textPos;

		private readonly int textLen;

		private char ch;

		private Token token;

		public ExpressionParser(ParameterExpression[] parameters, string expression, object[] values)
		{
			if (expression == null)
				throw new ArgumentNullException("expression");
			if (keywords == null)
				keywords = CreateKeywords();
			symbols = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			literals = new Dictionary<System.Linq.Expressions.Expression, string>();
			if (parameters != null)
				ProcessParameters(parameters);
			if (values != null)
				ProcessValues(values);
			textField = expression;
			textLen = textField.Length;
			SetTextPos(0);
			NextToken();
		}

		private void ProcessParameters(ParameterExpression[] parameters)
		{
			foreach (ParameterExpression pe in parameters)
			{
				if (!String.IsNullOrEmpty(pe.Name))
					AddSymbol(pe.Name, pe);
			}
			if (parameters.Length == 1 && String.IsNullOrEmpty(parameters[0].Name))
				it = parameters[0];
		}

		private void ProcessValues(object[] values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				object value = values[i];
				if (i == values.Length - 1 && value is IDictionary<string, object>)
					externals = (IDictionary<string, object>)value;
				else
					AddSymbol("@" + i.ToString(CultureInfo.InvariantCulture), value);
			}
		}

		private void AddSymbol(string name, object value)
		{
			if (symbols.ContainsKey(name))
				throw ParseError(Res.DuplicateIdentifier, name);
			symbols.Add(name, value);
		}

		public System.Linq.Expressions.Expression Parse(Type resultType)
		{
			int exprPos = token.pos;
			System.Linq.Expressions.Expression expr = ParseExpression();
			if (resultType != null)
			{
				if ((expr = PromoteExpression(expr, resultType, true)) == null)
					throw ParseError(exprPos, Res.ExpressionTypeMismatch, GetTypeName(resultType));
			}
			ValidateToken(TokenId.End, Res.SyntaxError);
			return expr;
		}

#pragma warning disable 0219
		public IEnumerable<DynamicOrdering> ParseOrdering()
		{
			var orderings = new List<DynamicOrdering>();
			while (true)
			{
				System.Linq.Expressions.Expression expr = ParseExpression();
				bool ascending = true;
				if (TokenIdentifierIs("asc") || TokenIdentifierIs("ascending"))
					NextToken();
				else if (TokenIdentifierIs("desc") || TokenIdentifierIs("descending"))
				{
					NextToken();
					ascending = false;
				}
				orderings.Add(new DynamicOrdering
				{
					Selector = expr,
					Ascending = ascending
				});
				if (token.id != TokenId.Comma)
					break;
				NextToken();
			}
			ValidateToken(TokenId.End, Res.SyntaxError);
			return orderings;
		}
#pragma warning restore 0219

		// ?: operator
		private System.Linq.Expressions.Expression ParseExpression()
		{
			int errorPos = token.pos;
			System.Linq.Expressions.Expression expr = ParseLogicalOr();
			if (token.id == TokenId.Question)
			{
				NextToken();
				System.Linq.Expressions.Expression expr1 = ParseExpression();
				ValidateToken(TokenId.Colon, Res.ColonExpected);
				NextToken();
				System.Linq.Expressions.Expression expr2 = ParseExpression();
				expr = GenerateConditional(expr, expr1, expr2, errorPos);
			}
			return expr;
		}

		// ||, or operator
		private System.Linq.Expressions.Expression ParseLogicalOr()
		{
			System.Linq.Expressions.Expression left = ParseLogicalAnd();
			while (token.id == TokenId.DoubleBar || TokenIdentifierIs("or"))
			{
				Token op = token;
				NextToken();
				System.Linq.Expressions.Expression right = ParseLogicalAnd();
				CheckAndPromoteOperands(typeof(ILogicalSignatures), op.text, ref left, ref right, op.pos);
				left = System.Linq.Expressions.Expression.OrElse(left, right);
			}
			return left;
		}

		// &&, and operator
		private System.Linq.Expressions.Expression ParseLogicalAnd()
		{
			System.Linq.Expressions.Expression left = ParseComparison();
			while (token.id == TokenId.DoubleAmphersand || TokenIdentifierIs("and"))
			{
				Token op = token;
				NextToken();
				System.Linq.Expressions.Expression right = ParseComparison();
				CheckAndPromoteOperands(typeof(ILogicalSignatures), op.text, ref left, ref right, op.pos);
				left = System.Linq.Expressions.Expression.AndAlso(left, right);
			}
			return left;
		}

		// =, ==, !=, <>, >, >=, <, <= operators
		System.Linq.Expressions.Expression ParseComparison()
		{
			System.Linq.Expressions.Expression left = ParseAdditive();
			while (token.id == TokenId.Equal || token.id == TokenId.DoubleEqual || token.id == TokenId.ExclamationEqual || token.id == TokenId.LessGreater || token.id == TokenId.GreaterThan || token.id == TokenId.GreaterThanEqual || token.id == TokenId.LessThan || token.id == TokenId.LessThanEqual)
			{
				Token op = token;
				NextToken();
				System.Linq.Expressions.Expression right = ParseAdditive();
				bool isEquality = op.id == TokenId.Equal || op.id == TokenId.DoubleEqual || op.id == TokenId.ExclamationEqual || op.id == TokenId.LessGreater;
				if (isEquality && !left.Type.IsValueType && !right.Type.IsValueType)
				{
					if (left.Type != right.Type)
					{
						if (left.Type.IsAssignableFrom(right.Type))
							right = System.Linq.Expressions.Expression.Convert(right, left.Type);
						else if (right.Type.IsAssignableFrom(left.Type))
							left = System.Linq.Expressions.Expression.Convert(left, right.Type);
						else
							throw IncompatibleOperandsError(op.text, left, right, op.pos);
					}
				}
				else if (IsEnumType(left.Type) || IsEnumType(right.Type))
				{
					if (left.Type != right.Type)
					{
						System.Linq.Expressions.Expression e;
						if ((e = PromoteExpression(right, left.Type, true)) != null)
							right = e;
						else if ((e = PromoteExpression(left, right.Type, true)) != null)
							left = e;
						else
							throw IncompatibleOperandsError(op.text, left, right, op.pos);
					}
				}
				else
					CheckAndPromoteOperands(isEquality ? typeof(IEqualitySignatures) : typeof(IRelationalSignatures), op.text, ref left, ref right, op.pos);
				switch (op.id)
				{
					case TokenId.Equal:
					case TokenId.DoubleEqual:
						left = GenerateEqual(left, right);
						break;
					case TokenId.ExclamationEqual:
					case TokenId.LessGreater:
						left = GenerateNotEqual(left, right);
						break;
					case TokenId.GreaterThan:
						left = GenerateGreaterThan(left, right);
						break;
					case TokenId.GreaterThanEqual:
						left = GenerateGreaterThanEqual(left, right);
						break;
					case TokenId.LessThan:
						left = GenerateLessThan(left, right);
						break;
					case TokenId.LessThanEqual:
						left = GenerateLessThanEqual(left, right);
						break;
				}
			}
			return left;
		}

		// +, -, & operators
		private System.Linq.Expressions.Expression ParseAdditive()
		{
			System.Linq.Expressions.Expression left = ParseMultiplicative();
			while (token.id == TokenId.Plus || token.id == TokenId.Minus || token.id == TokenId.Amphersand)
			{
				Token op = token;
				NextToken();
				System.Linq.Expressions.Expression right = ParseMultiplicative();
				switch (op.id)
				{
					case TokenId.Plus:
						if (left.Type == typeof(string) || right.Type == typeof(string))
							goto case TokenId.Amphersand;
						CheckAndPromoteOperands(typeof(IAddSignatures), op.text, ref left, ref right, op.pos);
						left = GenerateAdd(left, right);
						break;
					case TokenId.Minus:
						CheckAndPromoteOperands(typeof(ISubtractSignatures), op.text, ref left, ref right, op.pos);
						left = GenerateSubtract(left, right);
						break;
					case TokenId.Amphersand:
						left = GenerateStringConcat(left, right);
						break;
				}
			}
			return left;
		}

		// *, /, %, mod operators
		private System.Linq.Expressions.Expression ParseMultiplicative()
		{
			System.Linq.Expressions.Expression left = ParseUnary();
			while (token.id == TokenId.Asterisk || token.id == TokenId.Slash || token.id == TokenId.Percent || TokenIdentifierIs("mod"))
			{
				Token op = token;
				NextToken();
				System.Linq.Expressions.Expression right = ParseUnary();
				CheckAndPromoteOperands(typeof(IArithmeticSignatures), op.text, ref left, ref right, op.pos);
				switch (op.id)
				{
					case TokenId.Asterisk:
						left = System.Linq.Expressions.Expression.Multiply(left, right);
						break;
					case TokenId.Slash:
						left = System.Linq.Expressions.Expression.Divide(left, right);
						break;
					case TokenId.Percent:
					case TokenId.Identifier:
						left = System.Linq.Expressions.Expression.Modulo(left, right);
						break;
				}
			}
			return left;
		}

		// -, !, not unary operators
		private System.Linq.Expressions.Expression ParseUnary()
		{
			if (token.id == TokenId.Minus || token.id == TokenId.Exclamation || TokenIdentifierIs("not"))
			{
				Token op = token;
				NextToken();
				if (op.id == TokenId.Minus && (token.id == TokenId.IntegerLiteral || token.id == TokenId.RealLiteral))
				{
					token.text = "-" + token.text;
					token.pos = op.pos;
					return ParsePrimary();
				}
				System.Linq.Expressions.Expression expr = ParseUnary();
				if (op.id == TokenId.Minus)
				{
					CheckAndPromoteOperand(typeof(INegationSignatures), op.text, ref expr, op.pos);
					expr = System.Linq.Expressions.Expression.Negate(expr);
				}
				else
				{
					CheckAndPromoteOperand(typeof(INotSignatures), op.text, ref expr, op.pos);
					expr = System.Linq.Expressions.Expression.Not(expr);
				}
				return expr;
			}
			return ParsePrimary();
		}

		private System.Linq.Expressions.Expression ParsePrimary()
		{
			System.Linq.Expressions.Expression expr = ParsePrimaryStart();
			while (true)
			{
				if (token.id == TokenId.Dot)
				{
					NextToken();
					expr = ParseMemberAccess(null, expr);
				}
				else if (token.id == TokenId.OpenBracket)
					expr = ParseElementAccess(expr);
				else
					break;
			}
			return expr;
		}

		private System.Linq.Expressions.Expression ParsePrimaryStart()
		{
			switch (token.id)
			{
				case TokenId.Identifier:
					return ParseIdentifier();
				case TokenId.StringLiteral:
					return ParseStringLiteral();
				case TokenId.IntegerLiteral:
					return ParseIntegerLiteral();
				case TokenId.RealLiteral:
					return ParseRealLiteral();
				case TokenId.OpenParen:
					return ParseParenExpression();
				default:
					throw ParseError(Res.ExpressionExpected);
			}
		}

		private System.Linq.Expressions.Expression ParseStringLiteral()
		{
			ValidateToken(TokenId.StringLiteral);
			char quote = token.text[0];
			string s = token.text.Substring(1, token.text.Length - 2);
			int start = 0;
			while (true)
			{
				int i = s.IndexOf(quote, start);
				if (i < 0)
					break;
				s = s.Remove(i, 1);
				start = i + 1;
			}
			if (quote == '\'')
			{
				if (s.Length != 1)
					throw ParseError(Res.InvalidCharacterLiteral);
				NextToken();
				return CreateLiteral(s[0], s);
			}
			NextToken();
			return CreateLiteral(s, s);
		}

		private System.Linq.Expressions.Expression ParseIntegerLiteral()
		{
			ValidateToken(TokenId.IntegerLiteral);
			string text = token.text;
			if (text[0] != '-')
			{
				ulong value;
				if (!UInt64.TryParse(text, out value))
					throw ParseError(Res.InvalidIntegerLiteral, text);
				NextToken();
				if (value <= Int32.MaxValue)
					return CreateLiteral((int)value, text);
				if (value <= UInt32.MaxValue)
					return CreateLiteral((uint)value, text);
				if (value <= Int64.MaxValue)
					return CreateLiteral((long)value, text);
				return CreateLiteral(value, text);
			}
			else
			{
				long value;
				if (!Int64.TryParse(text, out value))
					throw ParseError(Res.InvalidIntegerLiteral, text);
				NextToken();
				if (value >= Int32.MinValue && value <= Int32.MaxValue)
					return CreateLiteral((int)value, text);
				return CreateLiteral(value, text);
			}
		}

		private System.Linq.Expressions.Expression ParseRealLiteral()
		{
			ValidateToken(TokenId.RealLiteral);
			string text = token.text;
			object value = null;
			char last = text[text.Length - 1];
			if (last == 'F' || last == 'f')
			{
				float f;
				if (Single.TryParse(text.Substring(0, text.Length - 1), out f))
					value = f;
			}
			else
			{
				double d;
				if (Double.TryParse(text, out d))
					value = d;
			}
			if (value == null)
				throw ParseError(Res.InvalidRealLiteral, text);
			NextToken();
			return CreateLiteral(value, text);
		}

		private System.Linq.Expressions.Expression CreateLiteral(object value, string text)
		{
			ConstantExpression expr = System.Linq.Expressions.Expression.Constant(value);
			literals.Add(expr, text);
			return expr;
		}

		private System.Linq.Expressions.Expression ParseParenExpression()
		{
			ValidateToken(TokenId.OpenParen, Res.OpenParenExpected);
			NextToken();
			System.Linq.Expressions.Expression e = ParseExpression();
			ValidateToken(TokenId.CloseParen, Res.CloseParenOrOperatorExpected);
			NextToken();
			return e;
		}

		private System.Linq.Expressions.Expression ParseIdentifier()
		{
			ValidateToken(TokenId.Identifier);
			object value;
			if (keywords.TryGetValue(token.text, out value))
			{
				if (value is Type)
					return ParseTypeAccess((Type)value);
				if (Equals(value, keywordIt))
					return ParseIt();
				if (Equals(value, keywordIif))
					return ParseIif();
				if (Equals(value, keywordNew))
					return ParseNew();
				NextToken();
				return (System.Linq.Expressions.Expression)value;
			}
			if (symbols.TryGetValue(token.text, out value) || externals != null && externals.TryGetValue(token.text, out value))
			{
				var expr = value as System.Linq.Expressions.Expression;
				if (expr == null)
					expr = System.Linq.Expressions.Expression.Constant(value);
				else
				{
					var lambda = expr as LambdaExpression;
					if (lambda != null)
						return ParseLambdaInvocation(lambda);
				}
				NextToken();
				return expr;
			}
			if (it != null)
				return ParseMemberAccess(null, it);
			throw ParseError(Res.UnknownIdentifier, token.text);
		}

		private System.Linq.Expressions.Expression ParseIt()
		{
			if (it == null)
				throw ParseError(Res.NoItInScope);
			NextToken();
			return it;
		}

		private System.Linq.Expressions.Expression ParseIif()
		{
			int errorPos = token.pos;
			NextToken();
			System.Linq.Expressions.Expression[] args = ParseArgumentList();
			if (args.Length != 3)
				throw ParseError(errorPos, Res.IifRequiresThreeArgs);
			return GenerateConditional(args[0], args[1], args[2], errorPos);
		}

		private System.Linq.Expressions.Expression GenerateConditional(System.Linq.Expressions.Expression test, System.Linq.Expressions.Expression expr1, System.Linq.Expressions.Expression expr2, int errorPos)
		{
			if (test.Type != typeof(bool))
				throw ParseError(errorPos, Res.FirstExprMustBeBool);
			if (expr1.Type != expr2.Type)
			{
				System.Linq.Expressions.Expression expr1as2 = expr2 != nullLiteral ? PromoteExpression(expr1, expr2.Type, true) : null;
				System.Linq.Expressions.Expression expr2as1 = expr1 != nullLiteral ? PromoteExpression(expr2, expr1.Type, true) : null;
				if (expr1as2 != null && expr2as1 == null)
					expr1 = expr1as2;
				else if (expr2as1 != null && expr1as2 == null)
					expr2 = expr2as1;
				else
				{
					string type1 = expr1 != nullLiteral ? expr1.Type.Name : "null";
					string type2 = expr2 != nullLiteral ? expr2.Type.Name : "null";
					if (expr1as2 != null && expr2as1 != null)
						throw ParseError(errorPos, Res.BothTypesConvertToOther, type1, type2);
					throw ParseError(errorPos, Res.NeitherTypeConvertsToOther, type1, type2);
				}
			}
			return System.Linq.Expressions.Expression.Condition(test, expr1, expr2);
		}

		private System.Linq.Expressions.Expression ParseNew()
		{
			NextToken();
			ValidateToken(TokenId.OpenParen, Res.OpenParenExpected);
			NextToken();
			var properties = new List<DynamicProperty>();
			var expressions = new List<System.Linq.Expressions.Expression>();
			while (true)
			{
				int exprPos = token.pos;
				System.Linq.Expressions.Expression expr = ParseExpression();
				string propName;
				if (TokenIdentifierIs("as"))
				{
					NextToken();
					propName = GetIdentifier();
					NextToken();
				}
				else
				{
					var me = expr as MemberExpression;
					if (me == null)
						throw ParseError(exprPos, Res.MissingAsClause);
					propName = me.Member.Name;
				}
				expressions.Add(expr);
				properties.Add(new DynamicProperty(propName, expr.Type));
				if (token.id != TokenId.Comma)
					break;
				NextToken();
			}
			ValidateToken(TokenId.CloseParen, Res.CloseParenOrCommaExpected);
			NextToken();
			Type type = DynamicExpression.CreateClass(properties);
			var bindings = new MemberBinding[properties.Count];
			for (int i = 0; i < bindings.Length; i++)
				bindings[i] = System.Linq.Expressions.Expression.Bind(type.GetProperty(properties[i].Name), expressions[i]);
			return System.Linq.Expressions.Expression.MemberInit(System.Linq.Expressions.Expression.New(type), bindings);
		}

		private System.Linq.Expressions.Expression ParseLambdaInvocation(LambdaExpression lambda)
		{
			int errorPos = token.pos;
			NextToken();
			System.Linq.Expressions.Expression[] args = ParseArgumentList();
			MethodBase method;
			if (FindMethod(lambda.Type, "Invoke", false, args, out method) != 1)
				throw ParseError(errorPos, Res.ArgsIncompatibleWithLambda);
			return System.Linq.Expressions.Expression.Invoke(lambda, args);
		}

		private System.Linq.Expressions.Expression ParseTypeAccess(Type type)
		{
			int errorPos = token.pos;
			NextToken();
			if (token.id == TokenId.Question)
			{
				if (!type.IsValueType || IsNullableType(type))
					throw ParseError(errorPos, Res.TypeHasNoNullableForm, GetTypeName(type));
				type = typeof(Nullable<>).MakeGenericType(type);
				NextToken();
			}
			if (token.id == TokenId.OpenParen)
			{
				System.Linq.Expressions.Expression[] args = ParseArgumentList();
				MethodBase method;
				switch (FindBestMethod(type.GetConstructors(), args, out method))
				{
					case 0:
						if (args.Length == 1)
							return GenerateConversion(args[0], type, errorPos);
						throw ParseError(errorPos, Res.NoMatchingConstructor, GetTypeName(type));
					case 1:
						return System.Linq.Expressions.Expression.New((ConstructorInfo)method, args);
					default:
						throw ParseError(errorPos, Res.AmbiguousConstructorInvocation, GetTypeName(type));
				}
			}
			ValidateToken(TokenId.Dot, Res.DotOrOpenParenExpected);
			NextToken();
			return ParseMemberAccess(type, null);
		}

		private static System.Linq.Expressions.Expression GenerateConversion(System.Linq.Expressions.Expression expr, Type type, int errorPos)
		{
			Type exprType = expr.Type;
			if (exprType == type)
				return expr;
			if (exprType.IsValueType && type.IsValueType)
			{
				if ((IsNullableType(exprType) || IsNullableType(type)) && GetNonNullableType(exprType) == GetNonNullableType(type))
					return System.Linq.Expressions.Expression.Convert(expr, type);
				if ((IsNumericType(exprType) || IsEnumType(exprType)) && (IsNumericType(type)) || IsEnumType(type))
					return System.Linq.Expressions.Expression.ConvertChecked(expr, type);
			}
			if (exprType.IsAssignableFrom(type) || type.IsAssignableFrom(exprType) || exprType.IsInterface || type.IsInterface)
				return System.Linq.Expressions.Expression.Convert(expr, type);
			throw ParseError(errorPos, Res.CannotConvertValue, GetTypeName(exprType), GetTypeName(type));
		}

		private System.Linq.Expressions.Expression ParseMemberAccess(Type type, System.Linq.Expressions.Expression instance)
		{
			if (instance != null)
				type = instance.Type;
			int errorPos = token.pos;
			string id = GetIdentifier();
			NextToken();
			if (token.id == TokenId.OpenParen)
			{
				if (instance != null && type != typeof(string))
				{
					Type enumerableType = FindGenericType(typeof(IEnumerable<>), type);
					if (enumerableType != null)
					{
						Type elementType = enumerableType.GetGenericArguments()[0];
						return ParseAggregate(instance, elementType, id, errorPos);
					}
				}
				System.Linq.Expressions.Expression[] args = ParseArgumentList();
				MethodBase mb;
				switch (FindMethod(type, id, instance == null, args, out mb))
				{
					case 0:
						throw ParseError(errorPos, Res.NoApplicableMethod, id, GetTypeName(type));
					case 1:
						var method = (MethodInfo)mb;
						//Removed to avoid that only specific types are able to access methods/properties
						//if (!IsPredefinedType(method.DeclaringType))
						//    throw ParseError(errorPos, Res.MethodsAreInaccessible, GetTypeName(method.DeclaringType));
						if (method.ReturnType == typeof(void))
							throw ParseError(errorPos, Res.MethodIsVoid, id, GetTypeName(method.DeclaringType));
						//It seems that method validation rejects parameters that are not exactly of the required type,
						//so it is better to cast them... if the cast is invalid it will fail anyway...
						var parameters = method.GetParameters();
						for (int i = 0; i < parameters.Length; i++)
						{
							if (parameters[i].ParameterType != args[i].Type)
								args[i] = System.Linq.Expressions.Expression.Convert(args[i], parameters[i].ParameterType);
						}
						return System.Linq.Expressions.Expression.Call(instance, method, args);
					default:
						throw ParseError(errorPos, Res.AmbiguousMethodInvocation, id, GetTypeName(type));
				}
			}
			else
			{
				MemberInfo member = FindPropertyOrField(type, id, instance == null);
				if (member == null)
					throw ParseError(errorPos, Res.UnknownPropertyOrField, id, GetTypeName(type));
				return member is PropertyInfo ? System.Linq.Expressions.Expression.Property(instance, (PropertyInfo)member) : System.Linq.Expressions.Expression.Field(instance, (FieldInfo)member);
			}
		}

		private static Type FindGenericType(Type generic, Type type)
		{
			while (type != null && type != typeof(object))
			{
				if (type.IsGenericType && type.GetGenericTypeDefinition() == generic)
					return type;
				if (generic.IsInterface)
				{
					foreach (Type intfType in type.GetInterfaces())
					{
						Type found = FindGenericType(generic, intfType);
						if (found != null)
							return found;
					}
				}
				type = type.BaseType;
			}
			return null;
		}

		private System.Linq.Expressions.Expression ParseAggregate(System.Linq.Expressions.Expression instance, Type elementType, string methodName, int errorPos)
		{
			ParameterExpression outerIt = it;
			ParameterExpression innerIt = System.Linq.Expressions.Expression.Parameter(elementType, "");
			it = innerIt;
			System.Linq.Expressions.Expression[] args = ParseArgumentList();
			it = outerIt;
			MethodBase signature;
			if (FindMethod(typeof(IEnumerableSignatures), methodName, false, args, out signature) != 1)
				throw ParseError(errorPos, Res.NoApplicableAggregate, methodName);
			Type[] typeArgs;
			if (signature.Name == "Min" || signature.Name == "Max")
				typeArgs = new[] { elementType, args[0].Type };
			else
				typeArgs = new[] { elementType };
			if (args.Length == 0)
				args = new[] { instance };
			else
				args = new[] { instance, System.Linq.Expressions.Expression.Lambda(args[0], innerIt) };
			return System.Linq.Expressions.Expression.Call(typeof(Enumerable), signature.Name, typeArgs, args);
		}

		private System.Linq.Expressions.Expression[] ParseArgumentList()
		{
			ValidateToken(TokenId.OpenParen, Res.OpenParenExpected);
			NextToken();
			System.Linq.Expressions.Expression[] args = token.id != TokenId.CloseParen ? ParseArguments() : new System.Linq.Expressions.Expression[0];
			ValidateToken(TokenId.CloseParen, Res.CloseParenOrCommaExpected);
			NextToken();
			return args;
		}

		private System.Linq.Expressions.Expression[] ParseArguments()
		{
			var argList = new List<System.Linq.Expressions.Expression>();
			while (true)
			{
				argList.Add(ParseExpression());
				if (token.id != TokenId.Comma)
					break;
				NextToken();
			}
			return argList.ToArray();
		}

		private System.Linq.Expressions.Expression ParseElementAccess(System.Linq.Expressions.Expression expr)
		{
			int errorPos = token.pos;
			ValidateToken(TokenId.OpenBracket, Res.OpenParenExpected);
			NextToken();
			System.Linq.Expressions.Expression[] args = ParseArguments();
			ValidateToken(TokenId.CloseBracket, Res.CloseBracketOrCommaExpected);
			NextToken();
			if (expr.Type.IsArray)
			{
				if (expr.Type.GetArrayRank() != 1 || args.Length != 1)
					throw ParseError(errorPos, Res.CannotIndexMultiDimArray);
				System.Linq.Expressions.Expression index = PromoteExpression(args[0], typeof(int), true);
				if (index == null)
					throw ParseError(errorPos, Res.InvalidIndex);
				return System.Linq.Expressions.Expression.ArrayIndex(expr, index);
			}
			else
			{
				MethodBase mb;
				switch (FindIndexer(expr.Type, args, out mb))
				{
					case 0:
						throw ParseError(errorPos, Res.NoApplicableIndexer, GetTypeName(expr.Type));
					case 1:
						return System.Linq.Expressions.Expression.Call(expr, (MethodInfo)mb, args);
					default:
						throw ParseError(errorPos, Res.AmbiguousIndexerInvocation, GetTypeName(expr.Type));
				}
			}
		}

		private static bool IsNullableType(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		private static Type GetNonNullableType(Type type)
		{
			return IsNullableType(type) ? type.GetGenericArguments()[0] : type;
		}

		private static string GetTypeName(Type type)
		{
			Type baseType = GetNonNullableType(type);
			string s = baseType.Name;
			if (type != baseType)
				s += '?';
			return s;
		}

		private static bool IsNumericType(Type type)
		{
			return GetNumericTypeKind(type) != 0;
		}

		private static bool IsSignedIntegralType(Type type)
		{
			return GetNumericTypeKind(type) == 2;
		}

		private static bool IsUnsignedIntegralType(Type type)
		{
			return GetNumericTypeKind(type) == 3;
		}

		private static int GetNumericTypeKind(Type type)
		{
			type = GetNonNullableType(type);
			if (type.IsEnum)
				return 0;
			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Char:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return 1;
				case TypeCode.SByte:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
					return 2;
				case TypeCode.Byte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
					return 3;
				default:
					return 0;
			}
		}

		private static bool IsEnumType(Type type)
		{
			return GetNonNullableType(type).IsEnum;
		}

		private void CheckAndPromoteOperand(Type signatures, string opName, ref System.Linq.Expressions.Expression expr, int errorPos)
		{
			var args = new[] { expr };
			MethodBase method;
			if (FindMethod(signatures, "F", false, args, out method) != 1)
				throw ParseError(errorPos, Res.IncompatibleOperand, opName, GetTypeName(args[0].Type));
			expr = args[0];
		}

		private void CheckAndPromoteOperands(Type signatures, string opName, ref System.Linq.Expressions.Expression left, ref System.Linq.Expressions.Expression right, int errorPos)
		{
			var args = new[] { left, right };
			MethodBase method;
			if (FindMethod(signatures, "F", false, args, out method) != 1)
				throw IncompatibleOperandsError(opName, left, right, errorPos);
			left = args[0];
			right = args[1];
		}

		private static Exception IncompatibleOperandsError(string opName, System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right, int pos)
		{
			return ParseError(pos, Res.IncompatibleOperands, opName, GetTypeName(left.Type), GetTypeName(right.Type));
		}

		private static MemberInfo FindPropertyOrField(Type type, string memberName, bool staticAccess)
		{
			BindingFlags flags = BindingFlags.Public | BindingFlags.DeclaredOnly | (staticAccess ? BindingFlags.Static : BindingFlags.Instance);
			foreach (Type t in SelfAndBaseTypes(type))
			{
				MemberInfo[] members = t.FindMembers(MemberTypes.Property | MemberTypes.Field, flags, Type.FilterNameIgnoreCase, memberName);
				if (members.Length != 0)
					return members[0];
			}
			return null;
		}

		private int FindMethod(Type type, string methodName, bool staticAccess, System.Linq.Expressions.Expression[] args, out MethodBase method)
		{
			BindingFlags flags = BindingFlags.Public | BindingFlags.DeclaredOnly | (staticAccess ? BindingFlags.Static : BindingFlags.Instance);
			foreach (Type t in SelfAndBaseTypes(type))
			{
				MemberInfo[] members = t.FindMembers(MemberTypes.Method, flags, Type.FilterNameIgnoreCase, methodName);
				int count = FindBestMethod(members.Cast<MethodBase>(), args, out method);
				if (count != 0)
					return count;
			}
			method = null;
			return 0;
		}

		private int FindIndexer(Type type, System.Linq.Expressions.Expression[] args, out MethodBase method)
		{
			foreach (Type t in SelfAndBaseTypes(type))
			{
				MemberInfo[] members = t.GetDefaultMembers();
				if (members.Length != 0)
				{
					IEnumerable<MethodBase> methods = members.OfType<PropertyInfo>().Select(p => (MethodBase)p.GetGetMethod()).Where(m => m != null);
					int count = FindBestMethod(methods, args, out method);
					if (count != 0)
						return count;
				}
			}
			method = null;
			return 0;
		}

		private static IEnumerable<Type> SelfAndBaseTypes(Type type)
		{
			if (type.IsInterface)
			{
				var types = new List<Type>();
				AddInterface(types, type);
				return types;
			}
			return SelfAndBaseClasses(type);
		}

		private static IEnumerable<Type> SelfAndBaseClasses(Type type)
		{
			while (type != null)
			{
				yield return type;
				type = type.BaseType;
			}
		}

		private static void AddInterface(List<Type> types, Type type)
		{
			if (!types.Contains(type))
			{
				types.Add(type);
				foreach (Type t in type.GetInterfaces())
					AddInterface(types, t);
			}
		}

		private class MethodData
		{
			#region Fields
			public System.Linq.Expressions.Expression[] Args;

			public MethodBase MethodBase;

			public ParameterInfo[] Parameters;
			#endregion
		}

		private int FindBestMethod(IEnumerable<MethodBase> methods, System.Linq.Expressions.Expression[] args, out MethodBase method)
		{
			MethodData[] applicable = methods.Select(m => new MethodData
			{
				MethodBase = m,
				Parameters = m.GetParameters()
			}).Where(m => IsApplicable(m, args)).ToArray();
			if (applicable.Length > 1)
				applicable = applicable.Where(m => applicable.All(n => m == n || IsBetterThan(args, m, n))).ToArray();
			if (applicable.Length == 1)
			{
				MethodData md = applicable[0];
				for (int i = 0; i < args.Length; i++)
					args[i] = md.Args[i];
				method = md.MethodBase;
			}
			else
				method = null;
			return applicable.Length;
		}

		private bool IsApplicable(MethodData method, System.Linq.Expressions.Expression[] args)
		{
			if (method.Parameters.Length != args.Length)
				return false;
			var promotedArgs = new System.Linq.Expressions.Expression[args.Length];
			for (int i = 0; i < args.Length; i++)
			{
				ParameterInfo pi = method.Parameters[i];
				if (pi.IsOut)
					return false;
				System.Linq.Expressions.Expression promoted = PromoteExpression(args[i], pi.ParameterType, false);
				if (promoted == null)
					return false;
				promotedArgs[i] = promoted;
			}
			method.Args = promotedArgs;
			return true;
		}

		private System.Linq.Expressions.Expression PromoteExpression(System.Linq.Expressions.Expression expr, Type type, bool exact)
		{
			if (expr.Type == type)
				return expr;
			if (expr is ConstantExpression)
			{
				var ce = (ConstantExpression)expr;
				if (ce == nullLiteral)
				{
					if (!type.IsValueType || IsNullableType(type))
						return System.Linq.Expressions.Expression.Constant(null, type);
				}
				else
				{
					string text;
					if (literals.TryGetValue(ce, out text))
					{
						Type target = GetNonNullableType(type);
						Object value = null;
						switch (Type.GetTypeCode(ce.Type))
						{
							case TypeCode.Int32:
							case TypeCode.UInt32:
							case TypeCode.Int64:
							case TypeCode.UInt64:
								value = ParseNumber(text, target);
								break;
							case TypeCode.Double:
								if (target == typeof(decimal))
									value = ParseNumber(text, target);
								break;
							case TypeCode.String:
								value = ParseEnum(text, target);
								break;
						}
						if (value != null)
							return System.Linq.Expressions.Expression.Constant(value, type);
					}
				}
			}
			if (IsCompatibleWith(expr.Type, type))
			{
				if (type.IsValueType || exact)
					return System.Linq.Expressions.Expression.Convert(expr, type);
				return expr;
			}
			return null;
		}

		private static object ParseNumber(string text, Type type)
		{
			switch (Type.GetTypeCode(GetNonNullableType(type)))
			{
				case TypeCode.SByte:
					sbyte sb;
					if (sbyte.TryParse(text, out sb))
						return sb;
					break;
				case TypeCode.Byte:
					byte b;
					if (byte.TryParse(text, out b))
						return b;
					break;
				case TypeCode.Int16:
					short s;
					if (short.TryParse(text, out s))
						return s;
					break;
				case TypeCode.UInt16:
					ushort us;
					if (ushort.TryParse(text, out us))
						return us;
					break;
				case TypeCode.Int32:
					int i;
					if (int.TryParse(text, out i))
						return i;
					break;
				case TypeCode.UInt32:
					uint ui;
					if (uint.TryParse(text, out ui))
						return ui;
					break;
				case TypeCode.Int64:
					long l;
					if (long.TryParse(text, out l))
						return l;
					break;
				case TypeCode.UInt64:
					ulong ul;
					if (ulong.TryParse(text, out ul))
						return ul;
					break;
				case TypeCode.Single:
					float f;
					if (float.TryParse(text, out f))
						return f;
					break;
				case TypeCode.Double:
					double d;
					if (double.TryParse(text, out d))
						return d;
					break;
				case TypeCode.Decimal:
					decimal e;
					if (decimal.TryParse(text, out e))
						return e;
					break;
			}
			return null;
		}

		private static object ParseEnum(string name, Type type)
		{
			if (type.IsEnum)
			{
				MemberInfo[] memberInfos = type.FindMembers(MemberTypes.Field, BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static, Type.FilterNameIgnoreCase, name);
				if (memberInfos.Length != 0)
					return ((FieldInfo)memberInfos[0]).GetValue(null);
			}
			return null;
		}

		private static bool IsCompatibleWith(Type source, Type target)
		{
			if (source == target)
				return true;
			if (!target.IsValueType)
				return target.IsAssignableFrom(source);
			Type st = GetNonNullableType(source);
			Type tt = GetNonNullableType(target);
			if (st != source && tt == target)
				return false;
			TypeCode sc = st.IsEnum ? TypeCode.Object : Type.GetTypeCode(st);
			TypeCode tc = tt.IsEnum ? TypeCode.Object : Type.GetTypeCode(tt);
			switch (sc)
			{
				case TypeCode.SByte:
					switch (tc)
					{
						case TypeCode.SByte:
						case TypeCode.Int16:
						case TypeCode.Int32:
						case TypeCode.Int64:
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Decimal:
							return true;
					}
					break;
				case TypeCode.Byte:
					switch (tc)
					{
						case TypeCode.Byte:
						case TypeCode.Int16:
						case TypeCode.UInt16:
						case TypeCode.Int32:
						case TypeCode.UInt32:
						case TypeCode.Int64:
						case TypeCode.UInt64:
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Decimal:
							return true;
					}
					break;
				case TypeCode.Int16:
					switch (tc)
					{
						case TypeCode.Int16:
						case TypeCode.Int32:
						case TypeCode.Int64:
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Decimal:
							return true;
					}
					break;
				case TypeCode.UInt16:
					switch (tc)
					{
						case TypeCode.UInt16:
						case TypeCode.Int32:
						case TypeCode.UInt32:
						case TypeCode.Int64:
						case TypeCode.UInt64:
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Decimal:
							return true;
					}
					break;
				case TypeCode.Int32:
					switch (tc)
					{
						case TypeCode.Int32:
						case TypeCode.Int64:
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Decimal:
							return true;
					}
					break;
				case TypeCode.UInt32:
					switch (tc)
					{
						case TypeCode.UInt32:
						case TypeCode.Int64:
						case TypeCode.UInt64:
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Decimal:
							return true;
					}
					break;
				case TypeCode.Int64:
					switch (tc)
					{
						case TypeCode.Int64:
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Decimal:
							return true;
					}
					break;
				case TypeCode.UInt64:
					switch (tc)
					{
						case TypeCode.UInt64:
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Decimal:
							return true;
					}
					break;
				case TypeCode.Single:
					switch (tc)
					{
						case TypeCode.Single:
						case TypeCode.Double:
							return true;
					}
					break;
				default:
					if (st == tt)
						return true;
					break;
			}
			return false;
		}

		private static bool IsBetterThan(System.Linq.Expressions.Expression[] args, MethodData m1, MethodData m2)
		{
			bool better = false;
			for (int i = 0; i < args.Length; i++)
			{
				int c = CompareConversions(args[i].Type, m1.Parameters[i].ParameterType, m2.Parameters[i].ParameterType);
				if (c < 0)
					return false;
				if (c > 0)
					better = true;
			}
			return better;
		}

		// Return 1 if s -> t1 is a better conversion than s -> t2
		// Return -1 if s -> t2 is a better conversion than s -> t1
		// Return 0 if neither conversion is better
		private static int CompareConversions(Type s, Type t1, Type t2)
		{
			if (t1 == t2)
				return 0;
			if (s == t1)
				return 1;
			if (s == t2)
				return -1;
			bool t1t2 = IsCompatibleWith(t1, t2);
			bool t2t1 = IsCompatibleWith(t2, t1);
			if (t1t2 && !t2t1)
				return 1;
			if (t2t1 && !t1t2)
				return -1;
			if (IsSignedIntegralType(t1) && IsUnsignedIntegralType(t2))
				return 1;
			if (IsSignedIntegralType(t2) && IsUnsignedIntegralType(t1))
				return -1;
			return 0;
		}

		private static System.Linq.Expressions.Expression GenerateEqual(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right)
		{
			return System.Linq.Expressions.Expression.Equal(left, right);
		}

		private static System.Linq.Expressions.Expression GenerateNotEqual(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right)
		{
			return System.Linq.Expressions.Expression.NotEqual(left, right);
		}

		private static System.Linq.Expressions.Expression GenerateGreaterThan(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right)
		{
			if (left.Type == typeof(string))
				return System.Linq.Expressions.Expression.GreaterThan(GenerateStaticMethodCall("Compare", left, right), System.Linq.Expressions.Expression.Constant(0));
			return System.Linq.Expressions.Expression.GreaterThan(left, right);
		}

		private static System.Linq.Expressions.Expression GenerateGreaterThanEqual(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right)
		{
			if (left.Type == typeof(string))
				return System.Linq.Expressions.Expression.GreaterThanOrEqual(GenerateStaticMethodCall("Compare", left, right), System.Linq.Expressions.Expression.Constant(0));
			return System.Linq.Expressions.Expression.GreaterThanOrEqual(left, right);
		}

		private static System.Linq.Expressions.Expression GenerateLessThan(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right)
		{
			if (left.Type == typeof(string))
				return System.Linq.Expressions.Expression.LessThan(GenerateStaticMethodCall("Compare", left, right), System.Linq.Expressions.Expression.Constant(0));
			return System.Linq.Expressions.Expression.LessThan(left, right);
		}

		private static System.Linq.Expressions.Expression GenerateLessThanEqual(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right)
		{
			if (left.Type == typeof(string))
				return System.Linq.Expressions.Expression.LessThanOrEqual(GenerateStaticMethodCall("Compare", left, right), System.Linq.Expressions.Expression.Constant(0));
			return System.Linq.Expressions.Expression.LessThanOrEqual(left, right);
		}

		private static System.Linq.Expressions.Expression GenerateAdd(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right)
		{
			if (left.Type == typeof(string) && right.Type == typeof(string))
				return GenerateStaticMethodCall("Concat", left, right);
			return System.Linq.Expressions.Expression.Add(left, right);
		}

		private static System.Linq.Expressions.Expression GenerateSubtract(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right)
		{
			return System.Linq.Expressions.Expression.Subtract(left, right);
		}

		private static System.Linq.Expressions.Expression GenerateStringConcat(System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right)
		{
			return System.Linq.Expressions.Expression.Call(null, typeof(string).GetMethod("Concat", new[] { typeof(object), typeof(object) }), new[] { left, right });
		}

		private static MethodInfo GetStaticMethod(string methodName, System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right)
		{
			return left.Type.GetMethod(methodName, new[] { left.Type, right.Type });
		}

		private static System.Linq.Expressions.Expression GenerateStaticMethodCall(string methodName, System.Linq.Expressions.Expression left, System.Linq.Expressions.Expression right)
		{
			return System.Linq.Expressions.Expression.Call(null, GetStaticMethod(methodName, left, right), new[] { left, right });
		}

		private void SetTextPos(int pos)
		{
			textPos = pos;
			ch = textPos < textLen ? textField[textPos] : '\0';
		}

		private void NextChar()
		{
			if (textPos < textLen)
				textPos++;
			ch = textPos < textLen ? textField[textPos] : '\0';
		}

		private void NextToken()
		{
			while (Char.IsWhiteSpace(ch))
				NextChar();
			TokenId t;
			int tokenPos = textPos;
			switch (ch)
			{
				case '!':
					NextChar();
					if (ch == '=')
					{
						NextChar();
						t = TokenId.ExclamationEqual;
					}
					else
						t = TokenId.Exclamation;
					break;
				case '%':
					NextChar();
					t = TokenId.Percent;
					break;
				case '&':
					NextChar();
					if (ch == '&')
					{
						NextChar();
						t = TokenId.DoubleAmphersand;
					}
					else
						t = TokenId.Amphersand;
					break;
				case '(':
					NextChar();
					t = TokenId.OpenParen;
					break;
				case ')':
					NextChar();
					t = TokenId.CloseParen;
					break;
				case '*':
					NextChar();
					t = TokenId.Asterisk;
					break;
				case '+':
					NextChar();
					t = TokenId.Plus;
					break;
				case ',':
					NextChar();
					t = TokenId.Comma;
					break;
				case '-':
					NextChar();
					t = TokenId.Minus;
					break;
				case '.':
					NextChar();
					t = TokenId.Dot;
					break;
				case '/':
					NextChar();
					t = TokenId.Slash;
					break;
				case ':':
					NextChar();
					t = TokenId.Colon;
					break;
				case '<':
					NextChar();
					if (ch == '=')
					{
						NextChar();
						t = TokenId.LessThanEqual;
					}
					else if (ch == '>')
					{
						NextChar();
						t = TokenId.LessGreater;
					}
					else
						t = TokenId.LessThan;
					break;
				case '=':
					NextChar();
					if (ch == '=')
					{
						NextChar();
						t = TokenId.DoubleEqual;
					}
					else
						t = TokenId.Equal;
					break;
				case '>':
					NextChar();
					if (ch == '=')
					{
						NextChar();
						t = TokenId.GreaterThanEqual;
					}
					else
						t = TokenId.GreaterThan;
					break;
				case '?':
					NextChar();
					t = TokenId.Question;
					break;
				case '[':
					NextChar();
					t = TokenId.OpenBracket;
					break;
				case ']':
					NextChar();
					t = TokenId.CloseBracket;
					break;
				case '|':
					NextChar();
					if (ch == '|')
					{
						NextChar();
						t = TokenId.DoubleBar;
					}
					else
						t = TokenId.Bar;
					break;
				case '"':
				case '\'':
					char quote = ch;
					do
					{
						NextChar();
						while (textPos < textLen && ch != quote)
							NextChar();
						if (textPos == textLen)
							throw ParseError(textPos, Res.UnterminatedStringLiteral);
						NextChar();
					}
					while (ch == quote);
					t = TokenId.StringLiteral;
					break;
				default:
					if (Char.IsLetter(ch) || ch == '@' || ch == '_')
					{
						do
						{
							NextChar();
						}
						while (Char.IsLetterOrDigit(ch) || ch == '_');
						t = TokenId.Identifier;
						break;
					}
					if (Char.IsDigit(ch))
					{
						t = TokenId.IntegerLiteral;
						do
						{
							NextChar();
						}
						while (Char.IsDigit(ch));
						if (ch == '.')
						{
							t = TokenId.RealLiteral;
							NextChar();
							ValidateDigit();
							do
							{
								NextChar();
							}
							while (Char.IsDigit(ch));
						}
						if (ch == 'E' || ch == 'e')
						{
							t = TokenId.RealLiteral;
							NextChar();
							if (ch == '+' || ch == '-')
								NextChar();
							ValidateDigit();
							do
							{
								NextChar();
							}
							while (Char.IsDigit(ch));
						}
						if (ch == 'F' || ch == 'f')
							NextChar();
						break;
					}
					if (textPos == textLen)
					{
						t = TokenId.End;
						break;
					}
					throw ParseError(textPos, Res.InvalidCharacter, ch);
			}
			token.id = t;
			token.text = textField.Substring(tokenPos, textPos - tokenPos);
			token.pos = tokenPos;
		}

		private bool TokenIdentifierIs(string id)
		{
			return token.id == TokenId.Identifier && String.Equals(id, token.text, StringComparison.OrdinalIgnoreCase);
		}

		private string GetIdentifier()
		{
			ValidateToken(TokenId.Identifier, Res.IdentifierExpected);
			string id = token.text;
			if (id.Length > 1 && id[0] == '@')
				id = id.Substring(1);
			return id;
		}

		private void ValidateDigit()
		{
			if (!Char.IsDigit(ch))
				throw ParseError(textPos, Res.DigitExpected);
		}

		private void ValidateToken(TokenId t, string errorMessage)
		{
			if (token.id != t)
				throw ParseError(errorMessage);
		}

		private void ValidateToken(TokenId t)
		{
			if (token.id != t)
				throw ParseError(Res.SyntaxError);
		}

		private Exception ParseError(string format, params object[] args)
		{
			return ParseError(token.pos, format, args);
		}

		private static Exception ParseError(int pos, string format, params object[] args)
		{
			return new ParseException(string.Format(CultureInfo.CurrentCulture, format, args), pos);
		}

		private static Dictionary<string, object> CreateKeywords()
		{
			var d = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			d.Add("true", trueLiteral);
			d.Add("false", falseLiteral);
			d.Add("null", nullLiteral);
			d.Add(keywordIt, keywordIt);
			d.Add(keywordIif, keywordIif);
			d.Add(keywordNew, keywordNew);
			foreach (Type type in predefinedTypes)
				d.Add(type.Name, type);
			return d;
		}
	}
}