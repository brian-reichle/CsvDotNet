// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace CsvDotNet.Benchmark;

[SimpleJob(RuntimeMoniker.Net462)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[MemoryDiagnoser]
public class CellValueDecoding
{
	[Benchmark]
	public string DecodeValue()
	{
		return CsvCellValue.DecodeValue(RawValue);
	}

	const string RawValue =
		"""
		"Foo, Bar
		Baz, Quux"
		""";
}
