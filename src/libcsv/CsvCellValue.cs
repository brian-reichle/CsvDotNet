// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.
namespace LibCsv;

public static partial class CsvCellValue
{
	public static string DecodeValue(ReadOnlySpan<char> rawValue)
	{
		if (rawValue.Length >= 2 && rawValue[0] == Delimiters.QuoteChar && rawValue[rawValue.Length - 1] == Delimiters.QuoteChar)
		{
			return DecodeQuotedContent(rawValue.Slice(1, rawValue.Length - 2));
		}
		else if (rawValue.Contains('"'))
		{
			InvalidCsvException.Throw();
		}

		return rawValue.ToString();
	}

	private static partial string DecodeQuotedContent(ReadOnlySpan<char> rawValue);
}
