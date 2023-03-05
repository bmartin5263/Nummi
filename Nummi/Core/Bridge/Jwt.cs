namespace Nummi.Core.Bridge; 

public record Jwt(string Value) {
    
    
    public static Jwt FromString(string Value) => new(Value);
    
    public override string ToString() {
        return Value;
    }
}