// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.
using System.Text;

namespace LibCsv;

public static class CsvCellValue
{
	public static string DecodeValue(ReadOnlySpan<char> rawValue)
	{
		if (!rawValue.Contains('"'))
		{
			return rawValue.ToString();
		}

		if (rawValue.Length >= 2 && rawValue[0] == Delimiters.QuoteChar && rawValue[rawValue.Length - 1] == Delimiters.QuoteChar)
		{
			rawValue = rawValue.Slice(1, rawValue.Length - 2);
		}
		else
		{
			throw new InvalidCsvException();
		}

		var i = rawValue.IndexOf(Delimiters.QuoteChar);

		if (i < 0)
		{
			return rawValue.ToString();
		}

		var builder = new StringBuilder();

		do
		{
			builder.Append(rawValue.Slice(0, i + 1));

			rawValue = rawValue.Slice(i);

			if (!rawValue.StartsWith(['"', '"']))
			{
				throw new InvalidCsvException();
			}

			rawValue = rawValue.Slice(2);

			i = rawValue.IndexOf(Delimiters.QuoteChar);
		}
		while (i >= 0);

		return builder.Append(rawValue).ToString();
	}
}
