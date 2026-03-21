// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.
using NUnit.Framework;

namespace LibCsv.Test;

[TestFixture]
public class CsvSpanReaderTest
{
	[Test]
	public void ReadEmpty()
	{
		var table = ReadDocument(string.Empty);

		// The comma and new-lines act as separators rather than terminators, so we can't
		// actually distinguish between:
		// * A document with no rows.
		// * A document with one row and no cells
		// * A document with one row and one empty cell.
		//
		// So we assume that all documents have at least one row and all rows have at least one cell.
		Assert.That(table.Length, Is.EqualTo(1));
		Assert.That(table[0], Is.EqualTo([string.Empty]));
	}

	[Test]
	public void RetainInternalCellWhitespace()
	{
		const string Document =
			"""
			   Foo   ,   Bar   
			""";

		var table = ReadDocument(Document);

		Assert.That(table.Length, Is.EqualTo(1));
		Assert.That(table[0], Is.EqualTo(["   Foo   ", "   Bar   "]));
	}

	[Test]
	public void ReadSimpleDocument()
	{
		const string Document =
			"""
			Foo,Bar
			Baz,
			Quux
			""";

		var table = ReadDocument(Document);

		Assert.That(table.Length, Is.EqualTo(3), "Should have 3 rows.");
		Assert.That(table[0], Is.EqualTo(["Foo", "Bar"]));
		Assert.That(table[1], Is.EqualTo(["Baz", string.Empty]));
		Assert.That(table[2], Is.EqualTo(["Quux"]));
	}

	[Test]
	public void ReadQuotedCell()
	{
		const string Document =
			"""
			"a,b","a""b","a
			b"
			"A,B","A""B","A
			B"
			""";

		var table = ReadDocument(Document);

		Assert.That(table.Length, Is.EqualTo(2));
		Assert.That(table[0], Is.EqualTo(["\"a,b\"", "\"a\"\"b\"", "\"a\r\nb\""]), "The raw span needs to include all relevant quotes, they will be decoded later.");
		Assert.That(table[1], Is.EqualTo(["\"A,B\"", "\"A\"\"B\"", "\"A\r\nB\""]), "The raw span needs to include all relevant quotes, they will be decoded later.");
	}

	[Test]
	public void SkipRemainingRow()
	{
		const string Document =
			"""
			A, B, "C
			D", "E,"
			F, G
			""";

		var reader = new CsvSpanReader(Document);

		Assert.That(reader.MoveNextRow(), Is.True, "Move to the first row.");
		Assert.That(reader.MoveNextCell(), Is.True, "Move to first cell on first row.");
		Assert.That(reader.RawCellValue.ToString(), Is.EqualTo("A"));

		Assert.That(reader.MoveNextRow(), Is.True, "Skip the remainder of the first row and move to the second.");
		Assert.That(reader.MoveNextCell(), Is.True, "Move to first cell on second row.");
		Assert.That(reader.RawCellValue.ToString(), Is.EqualTo("F"));

		Assert.That(reader.MoveNextRow(), Is.False, "There is no third row.");
	}

	[Test]
	public void UnterminatedQuote()
	{
		const string Document =
			"""
			"Foo, Bar
			Baz, Qux
			""";

		var reader = new CsvSpanReader(Document);
		reader.MoveNextRow();

		try
		{
			reader.MoveNextCell();
			Assert.Fail("Should have thrown an InvalidCsvException.");
		}
		catch (InvalidCsvException)
		{
		}
	}

	static string[][] ReadDocument(ReadOnlySpan<char> document)
	{
		var reader = new CsvSpanReader(document);
		var rowBuilder = new List<string>();
		var tableBuilder = new List<string[]>();

		while (reader.MoveNextRow())
		{
			rowBuilder.Clear();

			while (reader.MoveNextCell())
			{
				rowBuilder.Add(reader.RawCellValue.ToString());
			}

			tableBuilder.Add(rowBuilder.ToArray());
		}

		return tableBuilder.ToArray();
	}
}
