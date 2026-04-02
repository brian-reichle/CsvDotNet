// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.
#if !NET9_0_OR_GREATER
namespace CsvDotNet;

public partial class CsvCellValue
{
	private static partial string DecodeQuotedContent(ReadOnlySpan<char> rawValue)
	{
		var i = rawValue.IndexOf(Delimiters.QuoteChar);

		if (i < 0)
		{
			return rawValue.ToString();
		}

		var builder = StringBuilderPool.Rent();

		do
		{
			builder.Append(rawValue.Slice(0, i + 1));

			rawValue = rawValue.Slice(i);

			if (!rawValue.StartsWith([Delimiters.QuoteChar, Delimiters.QuoteChar]))
			{
				InvalidCsvException.Throw();
			}

			rawValue = rawValue.Slice(2);

			i = rawValue.IndexOf(Delimiters.QuoteChar);
		}
		while (i >= 0);

		return StringBuilderPool.ToStringAndReturn(builder.Append(rawValue));
	}
}
#endif
