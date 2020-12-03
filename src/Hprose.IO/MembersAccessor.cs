﻿/*--------------------------------------------------------*\
|                                                          |
|                          hprose                          |
|                                                          |
| Official WebSite: https://hprose.com                     |
|                                                          |
|  MembersAccessor.cs                                      |
|                                                          |
|  MembersAccessor class for C#.                           |
|                                                          |
|  LastModified: Feb 8, 2019                               |
|  Author: Ma Bingyao <andot@hprose.com>                   |
|                                                          |
\*________________________________________________________*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using static System.Reflection.BindingFlags;

namespace Hprose.IO {
    internal static class MembersAccessor {
        public static Dictionary<string, MemberInfo> GetMembers(Type type) {
            var members = new Dictionary<string, MemberInfo>();
            var flags = Public | Instance;
            var isDataContract = type.IsDefined(typeof(DataContractAttribute), false);
            if (isDataContract) {
                flags |= NonPublic;
            }
            var properties = type.GetProperties(flags);
            var ignoreDataMember = typeof(IgnoreDataMemberAttribute);
            string name;
            foreach (var property in properties) {
                var dataMember = Attribute.GetCustomAttribute(property, typeof(DataMemberAttribute), false) as DataMemberAttribute;
                if (property.CanRead && property.CanWrite &&
                    (!isDataContract || dataMember != null) &&
                    !property.IsDefined(ignoreDataMember, false) &&
                    property.GetIndexParameters().Length == 0 &&
                    !members.ContainsKey(name = Accessor.UnifiedName(dataMember?.Name ?? property.Name))) {
                    members[name] = property;
                }
            }
            var fields = type.GetFields(flags);
            foreach (var field in fields) {
                var dataMember = Attribute.GetCustomAttribute(field, typeof(DataMemberAttribute), false) as DataMemberAttribute;
                if ((!isDataContract || dataMember != null) &&
                    !field.IsDefined(ignoreDataMember, false) &&
                    !field.IsNotSerialized &&
                    !members.ContainsKey(name = Accessor.UnifiedName(dataMember?.Name ?? field.Name))) {
                    members[name] = field;
                }
            }
            return (from entry in members
                    orderby (Attribute.GetCustomAttribute(entry.Value, typeof(DataMemberAttribute), false) as DataMemberAttribute)?.Order ?? 0
                    select entry).ToDictionary(
                        pair => pair.Key,
                        pair => pair.Value,
                        StringComparer.OrdinalIgnoreCase
                    );
        }
    }
    public static class MembersAccessor<T> {
        public static Dictionary<string, MemberInfo> Members { get; } = MembersAccessor.GetMembers(typeof(T));
    }
}
