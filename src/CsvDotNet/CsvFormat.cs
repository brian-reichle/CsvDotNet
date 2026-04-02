// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.

#if NET
using DelimiterArray = System.Runtime.CompilerServices.InlineArray4<char>;
#else
using DelimiterArray = char[];
#endif

namespace CsvDotNet;

public sealed class CsvFormat
{
	public static CsvFormat Comma => GetCached(ref _comma, ',');
	public static CsvFormat Semicolon => GetCached(ref _semicolon, ';');

	public char SeparatorChar { get; }

	internal ReadOnlySpan<char> AllDelimiters => _delimiterChars;

	CsvFormat(char separator)
	{
		SeparatorChar = separator;

#if !NET
		_delimiterChars = new char[4];
#endif

		Init(_delimiterChars, separator);

		static void Init(Span<char> delimiters, char separator)
		{
			delimiters[0] = separator;
			delimiters[1] = Delimiters.QuoteChar;
			delimiters[2] = '\r';
			delimiters[3] = '\n';
		}
	}

	static CsvFormat GetCached(ref CsvFormat? cache, char separatorChar)
	{
		var result = cache;

		if (result != null)
		{
			return result;
		}

		Interlocked.CompareExchange(ref cache, new(separatorChar), null);
		return cache;
	}

	static CsvFormat? _comma;
	static CsvFormat? _semicolon;

	readonly DelimiterArray _delimiterChars;
}
