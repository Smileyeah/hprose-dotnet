﻿/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
| MockHandler.cs                                           |
|                                                          |
| MockHandler for C#.                                      |
|                                                          |
| LastModified: Mar 28, 2019                               |
| Author: Ma Bingyao <andot@hprose.com>                    |
|                                                          |
\*________________________________________________________*/

using System;
using System.IO;
using System.Threading.Tasks;

namespace Hprose.RPC {

    public class MockServer {
        public string Address { get; private set; }
        public MockServer(string address) {
            Address = address;
        }
        public void Close() {
            MockAgent.Cancel(Address);
        }
    }

    public class MockHandler : IHandler<MockServer> {
        public Service Service { get; private set; }
        public MockHandler(Service service) {
            Service = service;
        }
        public Task Bind(MockServer server) {
            MockAgent.Register(server.Address, Handler);
#if NET40
            return TaskEx.FromResult<object>(null);
#elif NET45 || NET451 || NET452
            return Task.FromResult<object>(null);
#else
            return Task.CompletedTask;
#endif
        }

        public async Task<Stream> Handler(string address, Stream request) {
            if (request.Length > Service.MaxRequestLength) {
                throw new Exception("Request entity too large");
            }
            var context = new ServiceContext(Service) {
                RemoteEndPoint = new MockEndPoint(address),
                LocalEndPoint = new MockEndPoint(address),
                Handler = this
            };
            return await Service.Handle(request, context).ConfigureAwait(false);
        }
    }
}
