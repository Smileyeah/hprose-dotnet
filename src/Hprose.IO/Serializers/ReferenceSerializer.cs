﻿/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ReferenceSerializer.cs                                  |
|                                                          |
|  hprose ReferenceSerializer class for C#.                |
|                                                          |
|  LastModified: Dec 13, 2018                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

namespace Hprose.IO.Serializers {
    public abstract class ReferenceSerializer<T> : Serializer<T> {

        // write your actual serialization code in sub class
        public override void Write(Writer writer, T obj) => writer.SetReference(obj);

        public override void Serialize(Writer writer, T obj) {
            if (obj != null) {
                if (!writer.WriteReference(obj)) {
                    Write(writer, obj);
                }
            }
            else {
                writer.Stream.WriteByte(Tags.TagNull);
            }
        }

    }
}