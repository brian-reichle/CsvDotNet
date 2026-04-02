// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.
#if NET9_0_OR_GREATER
using System.Diagnostics;

namespace CsvDotNet;

public partial class CsvCellValue
{
	private static partial string DecodeQuotedContent(ReadOnlySpan<char> rawValue)
	{
		// Net 9+ allows us to decode directly into the result string and avoid
		// any extra allocations & copy operations.
		//
		// Net 8 has all the right methods, but it's version of `string.Create<TState>()`
		// is missing `allow ref struct` on `TState` and so we can't use it here.
		var count = rawValue.Count(Delimiters.QuoteChar);

		if (count == 0)
		{
			return rawValue.ToString();
		}
		else if ((count & 1) == 1)
		{
			InvalidCsvException.Throw();
		}

		return string.Create(
			rawValue.Length - (count / 2),
			rawValue,
			WriteDecoded);
	}

	static void WriteDecoded(Span<char> target, ReadOnlySpan<char> source)
	{
		while (source.Length > 0)
		{
			var i = source.IndexOf(Delimiters.QuoteChar);

			if (i < 0)
			{
				Debug.Assert(target.Length == source.Length, "There are no more quotes, so the lengths should be identical.");
				source.CopyTo(target);
				break;
			}

			source.Slice(0, i + 1).CopyTo(target);

			target = target.Slice(i + 1);
			source = source.Slice(i + 1);

			if (source.Length == 0 || source[0] != Delimiters.QuoteChar)
			{
				InvalidCsvException.Throw();
			}

			source = source.Slice(1);
		}
	}
}
#endif
