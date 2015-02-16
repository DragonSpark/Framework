using System.Windows.Input;
using Xamarin.Forms;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	internal static class KeyboardExtensions
	{
		public static InputScope ToInputScope(this global::Xamarin.Forms.Keyboard self)
		{
			InputScope inputScope = new InputScope();
			InputScopeName inputScopeName = new InputScopeName();
			if (self == global::Xamarin.Forms.Keyboard.Default)
			{
				inputScopeName.NameValue = InputScopeNameValue.Default;
			}
			else
			{
				if (self == global::Xamarin.Forms.Keyboard.Chat)
				{
					inputScopeName.NameValue = InputScopeNameValue.Default;
				}
				else
				{
					if (self == global::Xamarin.Forms.Keyboard.Email)
					{
						inputScopeName.NameValue = InputScopeNameValue.EmailSmtpAddress;
					}
					else
					{
						if (self == global::Xamarin.Forms.Keyboard.Numeric)
						{
							inputScopeName.NameValue = InputScopeNameValue.Number;
						}
						else
						{
							if (self == global::Xamarin.Forms.Keyboard.Telephone)
							{
								inputScopeName.NameValue = InputScopeNameValue.TelephoneNumber;
							}
							else
							{
								if (self == global::Xamarin.Forms.Keyboard.Text)
								{
									inputScopeName.NameValue = InputScopeNameValue.Default;
								}
								else
								{
									if (self == global::Xamarin.Forms.Keyboard.Url)
									{
										inputScopeName.NameValue = InputScopeNameValue.Url;
									}
									else
									{
										if (self is CustomKeyboard)
										{
											CustomKeyboard customKeyboard = (CustomKeyboard)self;
											bool flag = (customKeyboard.Flags & KeyboardFlags.CapitalizeSentence) == KeyboardFlags.CapitalizeSentence;
											bool flag2 = (customKeyboard.Flags & KeyboardFlags.Spellcheck) == KeyboardFlags.Spellcheck;
											bool flag3 = (customKeyboard.Flags & KeyboardFlags.Suggestions) == KeyboardFlags.Suggestions;
											if (!flag && !flag2 && !flag3)
											{
												inputScopeName.NameValue = InputScopeNameValue.Default;
											}
											if (!flag && !flag2 && flag3)
											{
												inputScopeName.NameValue = InputScopeNameValue.Default;
											}
											if (!flag && flag2 && !flag3)
											{
												inputScopeName.NameValue = InputScopeNameValue.Default;
											}
											if (!flag && flag2 && flag3)
											{
												inputScopeName.NameValue = InputScopeNameValue.Default;
											}
											if (flag && !flag2 && !flag3)
											{
												inputScopeName.NameValue = InputScopeNameValue.Default;
											}
											if (flag && !flag2 && flag3)
											{
												inputScopeName.NameValue = InputScopeNameValue.Default;
											}
											if (flag && flag2 && !flag3)
											{
												inputScopeName.NameValue = InputScopeNameValue.Default;
											}
											if (flag && flag2 && flag3)
											{
												inputScopeName.NameValue = InputScopeNameValue.Default;
											}
										}
										else
										{
											inputScopeName.NameValue = InputScopeNameValue.Default;
										}
									}
								}
							}
						}
					}
				}
			}
			inputScope.Names.Add(inputScopeName);
			return inputScope;
		}
	}
}
