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
 * Accessor.cs                                            *
 *                                                        *
 * Accessor class for C#.                                 *
 *                                                        *
 * LastModified: Apr 25, 2018                             *
 * Author: Ma Bingyao <andot@hprose.com>                  *
 *                                                        *
\**********************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Hprose.IO.Accessors {
    public static class Accessor {
        public static string UnifiedName(string name) => char.ToLowerInvariant(name[0]) + name.Substring(1);
        public static Type GetMemberType(MemberInfo member) => member is FieldInfo ? ((FieldInfo)member).FieldType : ((PropertyInfo)member).PropertyType;

        private static readonly ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>> members = new ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>>();
        private static readonly ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>> fields = new ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>>();
        private static readonly ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>> properties = new ConcurrentDictionary<Type, Lazy<Dictionary<string, MemberInfo>>>();

        private static readonly Func<Type, Lazy<Dictionary<string, MemberInfo>>> fieldsFactory = (type) => new Lazy<Dictionary<string, MemberInfo>>(() => FieldsAccessor.GetFields(type));
        private static readonly Func<Type, Lazy<Dictionary<string, MemberInfo>>> propertiesFactory = (type) => new Lazy<Dictionary<string, MemberInfo>>(() => PropertiesAccessor.GetProperties(type));
        private static readonly Func<Type, Lazy<Dictionary<string, MemberInfo>>> membersFactory = (type) => new Lazy<Dictionary<string, MemberInfo>>(() => MembersAccessor.GetMembers(type));

        public static Dictionary<string, MemberInfo> GetMembers(Type type, HproseMode mode) {
            if (type.IsSerializable) {
                switch (mode) {
                    case HproseMode.FieldMode:
                        return fields.GetOrAdd(type, fieldsFactory).Value;
                    case HproseMode.PropertyMode:
                        return properties.GetOrAdd(type, propertiesFactory).Value;
                }
            }
            return members.GetOrAdd(type, membersFactory).Value;
        }
    }
}
