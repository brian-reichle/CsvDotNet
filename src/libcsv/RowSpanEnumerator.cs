// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.
namespace LibCsv;

ref struct RowSpanEnumerator
{
	public RowSpanEnumerator(ReadOnlySpan<char> document)
	{
		_alive = true;
		_remainingDocument = document;
	}

	public ReadOnlySpan<char> Current { get; private set; }

	public bool MoveNext()
	{
		if (!_alive)
		{
			return false;
		}

		var accumulator = 0;
		var tmp = _remainingDocument;

		while (true)
		{
			var i = tmp.IndexOfAny(['\r', '\n', Delimiters.QuoteChar]);

			if (i < 0)
			{
				Current = _remainingDocument;
				_remainingDocument = [];
				_alive = false;
				return true;
			}

			if (tmp[i] != Delimiters.QuoteChar)
			{
				accumulator += i;
				Current = _remainingDocument.Slice(0, accumulator);
				_remainingDocument = _remainingDocument.Slice(accumulator);
				_remainingDocument = _remainingDocument.Slice(_remainingDocument.StartsWith(['\r', '\n']) ? 2 : 1);
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
	ReadOnlySpan<char> _remainingDocument;
}
