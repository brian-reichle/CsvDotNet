// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.
#if !NET
namespace LibCsv;

static class MemoryExtensionsShims
{
	public static bool Contains<T>(this ReadOnlySpan<T> haystack, T needle)
		where T : struct, IEquatable<T>
		=> haystack.IndexOf(needle) >= 0;
}
#endif
