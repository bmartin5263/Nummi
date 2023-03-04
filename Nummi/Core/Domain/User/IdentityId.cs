using System.ComponentModel;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.User; 

[TypeConverter(typeof(IdentityIdConverter))]
public readonly record struct IdentityId(Guid Value) {
    public override string ToString() => Value.ToString("N");
    public string ToString(string format) => Value.ToString(format);
    public static IdentityId Generate() => new(Guid.NewGuid());
    public static IdentityId FromGuid(Guid id) => new(id);
    public static IdentityId FromString(string s) => new(Guid.Parse(s));
    public static IdentityId FromString(string s, string format) => new(Guid.ParseExact(s, format));
}