using System.ComponentModel;
using Nummi.Core.Util;

namespace Nummi.Core.Domain.Common; 

[TypeConverter(typeof(KsuidConverter))]
public record Ksuid(KSUID.Ksuid Value) : IComparable<Ksuid> {
    
    public virtual bool Equals(Ksuid? other) {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        var payloadEquals = Value.GetPayload().SequenceEqual(other.Value.GetPayload());
        var timestampEquals = Value.GetTimestamp().Equals(other.Value.GetTimestamp());
        return payloadEquals && timestampEquals;
    }

    public override int GetHashCode() {
        int hc = Value.GetPayload().Length;
        foreach (int val in Value.GetPayload()) {
            hc = unchecked(hc * 17 + val);
        }
        hc = unchecked(hc * 17 + (int) Value.GetTimestamp());
        return hc;
    }

    public static Ksuid Generate() {
        return new Ksuid(KSUID.Ksuid.Generate());
    }

    public static Ksuid FromString(string str) {
        return new Ksuid(KSUID.Ksuid.FromString(str));
    }

    public override string ToString() {
        return Value.ToString();
    }

    public int CompareTo(Ksuid? other) {
        return Value.GetTimestamp().CompareTo(other!.Value.GetTimestamp());
    }
    
    public static bool operator <(Ksuid id1, Ksuid id2) {
        return id1.CompareTo(id2) < 0;
    }
    
    public static bool operator >(Ksuid id1,Ksuid id2) {
        return id1.CompareTo(id2) > 0;
    }
    
    public static bool operator <=(Ksuid id1,Ksuid id2) {
        return id1.CompareTo(id2) <= 0;
    }
    
    public static bool operator >=(Ksuid id1,Ksuid id2) {
        return id1.CompareTo(id2) >= 0;
    }
}