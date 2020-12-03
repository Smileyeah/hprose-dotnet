﻿/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ReferenceReader.cs                                      |
|                                                          |
|  ReferenceReader class for C#.                           |
|                                                          |
|  LastModified: Jan 10, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;

namespace Hprose.IO {
    public static class ReferenceReader {
        public static byte[] ReadBytes(Reader reader) {
            var result = ValueReader.ReadBytes(reader.Stream);
            reader.AddReference(result);
            return result;
        }
        public static char[] ReadChars(Reader reader) {
            var result = ValueReader.ReadChars(reader.Stream);
            reader.AddReference(result);
            return result;
        }
        public static string ReadString(Reader reader) {
            var result = ValueReader.ReadString(reader.Stream);
            reader.AddReference(result);
            return result;
        }
        public static Guid ReadGuid(Reader reader) {
            var result = ValueReader.ReadGuid(reader.Stream);
            reader.AddReference(result);
            return result;
        }
        public static DateTime ReadDateTime(Reader reader) {
            var result = ValueReader.ReadDateTime(reader.Stream);
            reader.AddReference(result);
            return result;
        }
        public static DateTime ReadTime(Reader reader) {
            var result = ValueReader.ReadTime(reader.Stream);
            reader.AddReference(result);
            return result;
        }
        public static T[] ReadArray<T>(Reader reader) {
            Stream stream = reader.Stream;
            int count = ValueReader.ReadCount(stream);
            T[] a = new T[count];
            reader.AddReference(a);
            var deserializer = Deserializer<T>.Instance;
            for (int i = 0; i < count; ++i) {
                a[i] = deserializer.Deserialize(reader);
            }
            stream.ReadByte();
            return a;
        }

    }
}
