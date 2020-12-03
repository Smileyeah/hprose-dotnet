﻿/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  SingleConverter.cs                                      |
|                                                          |
|  hprose SingleConverter class for C#.                    |
|                                                          |
|  LastModified: Feb 21, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Numerics;

namespace Hprose.IO.Converters {
    internal static class SingleConverter {
        static SingleConverter() {
            Converter<bool, float>.convert = Convert.ToSingle;
            Converter<char, float>.convert = Convert.ToSingle;
            Converter<byte, float>.convert = Convert.ToSingle;
            Converter<sbyte, float>.convert = Convert.ToSingle;
            Converter<short, float>.convert = Convert.ToSingle;
            Converter<ushort, float>.convert = Convert.ToSingle;
            Converter<int, float>.convert = Convert.ToSingle;
            Converter<uint, float>.convert = Convert.ToSingle;
            Converter<long, float>.convert = Convert.ToSingle;
            Converter<ulong, float>.convert = Convert.ToSingle;
            Converter<double, float>.convert = Convert.ToSingle;
            Converter<decimal, float>.convert = Convert.ToSingle;
#if !NET35_CF
            Converter<DateTime, float>.convert = Convert.ToSingle;
#endif
            Converter<BigInteger, float>.convert = (value) => (float)value;
        }
        internal static void Initialize() { }
    }
}
