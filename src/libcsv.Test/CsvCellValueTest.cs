// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.
using NUnit.Framework;

namespace LibCsv.Test;

[TestFixture]
public class CsvCellValueTest
{
	[TestCase("", ExpectedResult = "")]
	[TestCase("\"\"", ExpectedResult = "")]
	[TestCase("Foo", ExpectedResult = "Foo")]
	[TestCase("\"Foo\"", ExpectedResult = "Foo")]
	[TestCase("   Foo   ", ExpectedResult = "   Foo   ")]
	[TestCase("\"\"\"Foo\"\"\"", ExpectedResult = "\"Foo\"")]
	[TestCase("\"Foo \"\"Bar\"\" Baz\"", ExpectedResult = "Foo \"Bar\" Baz")]
	[TestCase("\"A\r\nB\"", ExpectedResult = "A\r\nB")]
	public string DecodeValue(string value) => CsvCellValue.DecodeValue(value);

	[TestCase("\"")]
	[TestCase("\"[\"\"]")]
	[TestCase("\"[\" \"]\"")]
	public void DecodeValueInvalid(string value)
	{
		Assert.That(
			() => CsvCellValue.DecodeValue(value),
			Throws.InstanceOf<InvalidCsvException>());
	}
}
