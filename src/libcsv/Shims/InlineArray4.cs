// Copyright (c) Brian Reichle.  All Rights Reserved.  Licensed under the MIT License.  See LICENSE in the project root for license information.
#if NET && !NET10_0_OR_GREATER
namespace System.Runtime.CompilerServices;

[InlineArray(4)]
public struct InlineArray4<T>
{
	T _firstValue;
}
#endif
