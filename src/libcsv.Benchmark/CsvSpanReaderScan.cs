// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace LibCsv.Benchmark;

[SimpleJob(RuntimeMoniker.Net462)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[MemoryDiagnoser]
public class CsvSpanReaderScan
{
	[Benchmark]
	public int Scan()
	{
		var count = 0;
		var reader = new CsvSpanReader(Document);

		while (reader.MoveNextRow())
		{
			while (reader.MoveNextCell())
			{
				count++;
			}
		}

		return count;
	}

	const string Document =
		"""
		Lead, "Foo
		Bar
		Baz"
		"W, X, Y, Z", Magic
		""";
}
