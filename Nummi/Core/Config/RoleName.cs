namespace Nummi.Core.Config; 

public class RoleName {
    public static readonly RoleName User = new("User");
    public static readonly RoleName Admin = new("Admin");
    
    private readonly string value;
    
    private RoleName(string value) {
        this.value = value;
    }

    public override string ToString() {
        return value;
    }
}