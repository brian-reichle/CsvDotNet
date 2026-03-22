// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.

namespace LibCsv;

public sealed class CsvFormat
{
	public static CsvFormat Comma => GetCached(ref _comma, ',');
	public static CsvFormat Semicolon => GetCached(ref _semicolon, ';');

	public char SeparatorChar { get; }

	internal ReadOnlySpan<char> AllDelimiters => _delimiterChars;

	CsvFormat(char separator)
	{
		SeparatorChar = separator;
		_delimiterChars = [separator, Delimiters.QuoteChar, '\r', '\n'];
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

	readonly char[] _delimiterChars;
}
