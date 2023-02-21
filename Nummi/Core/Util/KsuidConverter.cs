using System.ComponentModel;
using System.Globalization;
using Nummi.Core.Domain.Common;

namespace Nummi.Core.Util; 

public class KsuidConverter : TypeConverter {
    
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }
    
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value) {
        return Ksuid.FromString((string)value);
    }
    
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType) {
        return (value as Ksuid)?.ToString();
    }

}