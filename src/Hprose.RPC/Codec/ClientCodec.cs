﻿/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  ClientCodec.cs                                          |
|                                                          |
|  ClientCodec class for C#.                               |
|                                                          |
|  LastModified: Jan 20, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using Hprose.IO;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Hprose.RPC.Codec {
    class ClientCodec : IClientCodec {
        public static IClientCodec Instance { get; } = new ClientCodec();
        public Stream Encode(string name, object[] args, ClientContext context) {
            var stream = new MemoryStream();
            var writer = new Writer(stream, context.Simple, context.Mode);
            if ((context.RequestHeaders as IDictionary<string, object>).Count > 0) {
                stream.WriteByte(Tags.TagHeader);
                writer.Serialize(context.RequestHeaders);
                writer.Reset();
            }
            stream.WriteByte(Tags.TagCall);
            writer.Serialize(name);
            if (args != null && args.Length > 0) {
                writer.Reset();
                writer.Serialize(args);
            }
            stream.WriteByte(Tags.TagEnd);
            stream.Position = 0;
            return stream;
        }
        public async Task<object> Decode(Stream response, ClientContext context) {
            MemoryStream stream;
            if (response is MemoryStream) {
                stream = (MemoryStream)response;
            }
            else {
                stream = new MemoryStream();
                await response.CopyToAsync(stream);
            }
            var reader = new Reader(stream, false, context.Mode) {
                LongType = context.LongType,
                RealType = context.RealType,
                CharType = context.CharType,
                ListType = context.ListType,
                DictType = context.DictType
            };
            var tag = stream.ReadByte();
            if (tag == Tags.TagHeader) {
                var headers = reader.Deserialize<ExpandoObject>();
                var responseHeaders = context.ResponseHeaders as IDictionary<string, object>;
                foreach (var pair in headers) {
                    responseHeaders[pair.Key] = pair.Value;
                }
                reader.Reset();
                tag = stream.ReadByte();
            }
            switch (tag) {
                case Tags.TagResult:
                    return reader.Deserialize(context.Type);
                case Tags.TagError:
                    throw new Exception(reader.Deserialize<string>());
                case Tags.TagEnd:
                    return null;
                default:
                    throw new Exception("Invalid response\r\n" + Encoding.UTF8.GetString(stream.ToArray()));
            }
        }
    }
}
