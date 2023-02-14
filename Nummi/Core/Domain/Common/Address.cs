namespace Nummi.Core.Domain.Common; 

public class Address {
    public string? Line1 { get; set; }
    public string? Line2 { get; set; }
    public string? City { get; set; } // locality
    public State? State { get; set; } // region
    public string? ZipCode { get; set; }
    public Country? Country { get; set; }
}