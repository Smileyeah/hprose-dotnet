﻿/**********************************************************\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: http://www.hprose.com/                 |
|                   http://www.hprose.org/                 |
|                                                          |
\**********************************************************/
/**********************************************************\
 *                                                        *
 * Writer.cs                                              *
 *                                                        *
 * hprose Writer class for C#.                            *
 *                                                        *
 * LastModified: Apr 1, 2018                              *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Generic;
using System.IO;

namespace Hprose.IO.Serializers {
    public class Writer {
        private readonly Stream _stream;
        private readonly WriterRefer _refer;
        private readonly HproseMode _mode;
        private readonly Dictionary<Type, int> _ref = new Dictionary<Type, int>();
        private int _last = 0;

        public Stream Stream => _stream;

        public Writer(Stream stream, HproseMode mode = HproseMode.MemberMode, bool simple = false) {
            _stream = stream;
            _mode = mode;
            _refer = simple ? null : new WriterRefer();
        }

        public void Serialize(object obj) => Serializer.Instance.Write(this, obj);

        public void Serialize<T>(T obj) => Serializer<T>.Instance.Write(this, obj);

        internal bool WriteRef(object obj) => _refer?.Write(_stream, obj) ?? false;

        internal void SetRef(object obj) => _refer?.Set(obj);

        public void Reset() {
            _refer?.Reset();
            _ref.Clear();
            _last = 0;
        }
    }
}
