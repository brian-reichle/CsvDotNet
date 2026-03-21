// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.
namespace LibCsv;

public ref struct CsvSpanReader
{
	public CsvSpanReader(ReadOnlySpan<char> csv)
	{
		_rows = new(csv);
	}

	public readonly ReadOnlySpan<char> RawCellValue => _cells.Current;

	public bool MoveNextRow()
	{
		if (!_rows.MoveNext())
		{
			return false;
		}

		_cells = new(_rows.Current);
		return true;
	}

	public bool MoveNextCell() => _cells.MoveNext();

	CellSpanEnumerator _cells;
	RowSpanEnumerator _rows;
}
