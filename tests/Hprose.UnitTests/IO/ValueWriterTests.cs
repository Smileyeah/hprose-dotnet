﻿using Hprose.IO;
using Hprose.RPC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace Hprose.UnitTests.IO {
    [TestClass]
    public class ValueWriterTests {
        private string WriteInt(int i) {
            using (var stream = new MemoryStream()) {
                ValueWriter.WriteInt(stream, i);
                var data = stream.GetArraySegment();
                return Encoding.ASCII.GetString(data.Array, data.Offset, data.Count);
            }
        }
        private string WriteInt(uint i) {
            using (var stream = new MemoryStream()) {
                ValueWriter.WriteInt(stream, i);
                var data = stream.GetArraySegment();
                return Encoding.ASCII.GetString(data.Array, data.Offset, data.Count);
            }
        }
        private string WriteInt(long i) {
            using (var stream = new MemoryStream()) {
                ValueWriter.WriteInt(stream, i);
                var data = stream.GetArraySegment();
                return Encoding.ASCII.GetString(data.Array, data.Offset, data.Count);
            }
        }
        private string WriteInt(ulong i) {
            using (var stream = new MemoryStream()) {
                ValueWriter.WriteInt(stream, i);
                var data = stream.GetArraySegment();
                return Encoding.ASCII.GetString(data.Array, data.Offset, data.Count);
            }
        }
        [TestMethod]
        public void TestWriteInt() {
            Assert.AreEqual("0", WriteInt((byte)0));
            Assert.AreEqual("0", WriteInt(0));
            Assert.AreEqual("1", WriteInt(1));
            Assert.AreEqual("9", WriteInt(9));
            Assert.AreEqual("123456789", WriteInt(123456789));
            Assert.AreEqual("-1", WriteInt(-1));
            Assert.AreEqual("-123456789", WriteInt(-123456789));
            Assert.AreEqual(int.MinValue.ToString(), WriteInt(int.MinValue));
            Assert.AreEqual(int.MaxValue.ToString(), WriteInt(int.MaxValue));
            Assert.AreEqual(uint.MaxValue.ToString(), WriteInt(uint.MaxValue));
            Assert.AreEqual(long.MinValue.ToString(), WriteInt(long.MinValue));
            Assert.AreEqual(long.MaxValue.ToString(), WriteInt(long.MaxValue));
            Assert.AreEqual(ulong.MaxValue.ToString(), WriteInt(ulong.MaxValue));
            Assert.AreEqual("-1234567890987654321", WriteInt(-1234567890987654321));
        }
    }
}
