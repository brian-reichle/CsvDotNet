// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.
#if !NET
using System.Runtime.Serialization;

namespace CsvDotNet;

[Serializable]
public partial class InvalidCsvException : Exception
{
	InvalidCsvException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
#endif
