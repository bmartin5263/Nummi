using JetBrains.Annotations;
using KSUID;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nummi.Core.Util;

namespace Nummi.Core.Database.EFCore;

[UsedImplicitly]
public class KsuidConverter : ValueConverter<Ksuid, string> {
    public KsuidConverter()
        : base(
            v => v.ToString(),
            v => Ksuid.FromString(v)
        )
    {
    }
}

[UsedImplicitly]
public class NummiJsonConverter<T> : ValueConverter<T, string> {
    public NummiJsonConverter()
        : base(
            v => Serializer.ToJson(v),
            v => Serializer.FromJson<T>(v)!
        )
    {
    }
}