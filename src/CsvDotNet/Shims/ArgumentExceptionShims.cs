// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.
#if !NET
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System;

static class ArgumentExceptionShims
{
	extension(ArgumentNullException)
	{
		public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		{
			if (argument == null)
			{
				ThrowArgumentNullException(paramName);
			}
		}
	}

	[DoesNotReturn]
	static void ThrowArgumentNullException(string? parameterName) => throw new ArgumentNullException(parameterName);
}
#endif
