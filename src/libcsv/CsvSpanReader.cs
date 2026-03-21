// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.
namespace LibCsv;

public ref struct CsvSpanReader
{
	public CsvSpanReader(ReadOnlySpan<char> csv)
	{
		_remainingDocument = csv;
		_status = Status.EndOfRow;
	}

	public ReadOnlySpan<char> RawCellValue { get; private set; }

	public bool MoveNextRow()
	{
		RawCellValue = [];

		switch (_status)
		{
			case Status.EndOfDocument:
				return false;

			case Status.EndOfRow:
				_status = Status.Ready;
				return true;
		}

		// MoveNextRow was called, but we have not yet read all cells in the current row, so skip ahead
		// to the end of the row (first un-quoted EOL).
		while (true)
		{
			var i = _remainingDocument.IndexOfAny([Delimiters.QuoteChar, '\r', '\n']);

			if (i < 0)
			{
				_status = Status.EndOfDocument;
				return false;
			}

			_remainingDocument = _remainingDocument.Slice(i);

			if (_remainingDocument[0] != Delimiters.QuoteChar)
			{
				_status = Status.Ready;
				_remainingDocument = _remainingDocument.Slice(_remainingDocument.StartsWith(['\r', '\n']) ? 2 : 1);
				return true;
			}

			_remainingDocument = _remainingDocument.Slice(i + 1);

			i = _remainingDocument.IndexOf(Delimiters.QuoteChar);

			if (i < 0)
			{
				InvalidCsvException.Throw();
			}

			_remainingDocument = _remainingDocument.Slice(i + 1);
		}
	}

	public bool MoveNextCell()
	{
		if (_status != Status.Ready)
		{
			return false;
		}

		var accumulator = 0;
		var tmp = _remainingDocument;

		while (true)
		{
			var i = tmp.IndexOfAny([Delimiters.QuoteChar, Delimiters.SeparatorChar, '\r', '\n']);

			if (i < 0)
			{
				RawCellValue = _remainingDocument;
				_remainingDocument = [];
				_status = Status.EndOfDocument;
				return true;
			}

			if (tmp[i] != Delimiters.QuoteChar)
			{
				accumulator += i;
				RawCellValue = _remainingDocument.Slice(0, accumulator);
				_remainingDocument = _remainingDocument.Slice(accumulator);

				if (_remainingDocument[0] == Delimiters.SeparatorChar)
				{
					_remainingDocument = _remainingDocument.Slice(1);
					return true;
				}

				_remainingDocument = _remainingDocument.Slice(_remainingDocument.StartsWith(['\r', '\n']) ? 2 : 1);
				_status = Status.EndOfRow;
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

	Status _status;
	ReadOnlySpan<char> _remainingDocument;

	enum Status
	{
		/// <summary>
		/// The entirety of the document has been read, there is nothing more.
		/// </summary>
		/// <remarks>
		/// EndOfDocument was specifically chosen to have the `0` value because we can't stop
		/// people from using CsvSpanReader's default constructor and using `0` here would
		/// mean that the state of such a constructed reader would default to "EndOfDocument"
		/// which means that both MoveNextRow and MoveNextCell return false without reading
		/// the defaulted _remainingDocument field.
		/// </remarks>
		EndOfDocument = 0,

		/// <summary>
		/// There is nothing left to read of the current row, but there is another row
		/// that we can start reading after MoveToNextRow is called.
		/// </summary>
		EndOfRow = 1,

		/// <summary>
		/// There is still more to read in the current row.
		/// </summary>
		Ready = 2,
	}
}
