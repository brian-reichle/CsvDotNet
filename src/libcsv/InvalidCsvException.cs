// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.
using System.Diagnostics.CodeAnalysis;

namespace LibCsv;

public sealed partial class InvalidCsvException : Exception
{
	public InvalidCsvException()
		: base("Invalid csv encountered.")
	{
	}

	public InvalidCsvException(string message)
		: base(message)
	{
	}

	public InvalidCsvException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	[DoesNotReturn]
	internal static void Throw() => throw new InvalidCsvException();
}
