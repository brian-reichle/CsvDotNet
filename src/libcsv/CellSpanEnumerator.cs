// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.
namespace LibCsv;

ref struct CellSpanEnumerator
{
	public CellSpanEnumerator(ReadOnlySpan<char> row)
	{
		_remainingRow = row;
		_alive = true;
	}

	public ReadOnlySpan<char> Current { get; private set; }

	public bool MoveNext()
	{
		if (!_alive)
		{
			return false;
		}

		var accumulator = 0;
		var tmp = _remainingRow;

		while (true)
		{
			var i = tmp.IndexOfAny([Delimiters.SeparatorChar, Delimiters.QuoteChar]);

			if (i < 0)
			{
				Current = _remainingRow;
				_remainingRow = [];
				_alive = false;
				return true;
			}

			if (tmp[i] != Delimiters.QuoteChar)
			{
				accumulator += i;
				Current = _remainingRow.Slice(0, accumulator);
				_remainingRow = _remainingRow.Slice(accumulator + 1);
				return true;
			}

			i++;
			accumulator += i;
			tmp = tmp.Slice(i);

			i = tmp.IndexOf(Delimiters.QuoteChar);

			if (i < 0)
			{
				InvalidCsvException.Throw();
			}

			i++;
			accumulator += i;
			tmp = tmp.Slice(i);
		}
	}

	bool _alive;
	ReadOnlySpan<char> _remainingRow;
}
