using System.ComponentModel;
using System.Text;

namespace Nummi.Core.Util; 

public static class StringUtil {

    public static string ToFormattedString<T>(this T self) {
        if (self == null) {
            return "null";
        }
        PropertyDescriptorCollection props = TypeDescriptor.GetProperties(self);
        StringBuilder sb = new StringBuilder();
        sb.Append(self.GetType().Name);
        sb.Append('(');
        foreach(PropertyDescriptor prop in props)
        {
            sb.Append($"{prop.Name}={prop.GetValue(self)}, ");
        }
        sb.Append(')');
        return sb.ToString();
    }
    
}