﻿/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  BytesSerializer.cs                                      |
|                                                          |
|  BytesSerializer class for C#.                           |
|                                                          |
|  LastModified: Jan 11, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Serializers {
    using static Tags;

    internal class BytesSerializer : ReferenceSerializer<byte[]> {
        public override void Write(Writer writer, byte[] obj) {
            base.Write(writer, obj);
            var stream = writer.Stream;
            stream.WriteByte(TagBytes);
            int length = obj.Length;
            if (length > 0) {
                ValueWriter.WriteInt(stream, length);
            }
            stream.WriteByte(TagQuote);
            stream.Write(obj, 0, length);
            stream.WriteByte(TagQuote);
        }
    }
}
