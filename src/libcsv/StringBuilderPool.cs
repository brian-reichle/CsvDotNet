// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.
using System.Text;

namespace LibCsv;

static class StringBuilderPool
{
	public static StringBuilder Rent()
	{
		var result = _cache ?? new();
		_cache = null;
		return result;
	}

	public static void Return(StringBuilder builder)
	{
		if (builder.Capacity < 300 && _cache == null)
		{
			builder.Clear();
			_cache = builder;
		}
	}

	public static string ToStringAndReturn(StringBuilder builder)
	{
		var result = builder.ToString();
		Return(builder);
		return result;
	}

	[ThreadStatic]
	static StringBuilder? _cache;
}
