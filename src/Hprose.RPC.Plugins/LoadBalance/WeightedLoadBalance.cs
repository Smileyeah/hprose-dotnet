﻿/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  WeightedLoadBalance.cs                                  |
|                                                          |
|  Weighted LoadBalance plugin for C#.                     |
|                                                          |
|  LastModified: Apr 26, 2019                              |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Hprose.RPC.Plugins.LoadBalance {
    public abstract class WeightedLoadBalance {
        protected readonly Uri[] uris;
        protected readonly int[] weights;
        public WeightedLoadBalance(IDictionary<string, int> uriList) {
            if (uriList == null) {
                throw new ArgumentNullException(nameof(uriList));
            }
            var n = uriList.Count;
            uris = new Uri[n];
            weights = new int[n];
            var i = 0;
            foreach (var pair in uriList) {
                uris[i] = new Uri(pair.Key);
                weights[i] = pair.Value;
                if (weights[i] <= 0) {
                    throw new ArgumentOutOfRangeException(nameof(uriList), "weight must be great than 0");
                }
                i++;
            }
            if (uris.Length == 0) {
                throw new ArgumentException("cannot be empty", nameof(uriList));
            }
        }
        public abstract Task<Stream> Handler(Stream request, Context context, NextIOHandler next);
    }
}
