namespace Nummi.Core.Domain.Common; 

public record Ksuid(KSUID.Ksuid Value) {
    
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
}