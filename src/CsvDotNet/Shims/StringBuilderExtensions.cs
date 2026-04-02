// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.
#if !NET
using System.Text;

namespace CsvDotNet;

static class StringBuilderExtensions
{
	public static unsafe StringBuilder Append(this StringBuilder builder, ReadOnlySpan<char> value)
	{
		fixed (char* ptr = value)
		{
			return builder.Append(ptr, value.Length);
		}
	}
}
#endif
