using System.Collections;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Primitives;

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

    public static string ToJoinedString<T, V>(this T self, char delimiter = ',', string prefix = "[", string suffix = "]")
        where T : IEnumerable<V> {
        return prefix + string.Join(delimiter, self) + suffix;
    }

    public static string ToJoinedString(
        this IEnumerable<KeyValuePair<string, IEnumerable<string>>> self, 
        string delimiter = ", ", 
        string prefix = "", 
        string suffix = ""
    ) {
        var sb = new StringBuilder();
        sb.Append(prefix);
        using var iter = self.GetEnumerator();
        if (iter.MoveNext()) {
            var keyValuePair = iter.Current;
            sb.Append(keyValuePair.Key);
            sb.Append('=');
            sb.Append(keyValuePair.Value.ToJoinedString<IEnumerable<string>, string>());
        }
        while (iter.MoveNext()) {
            sb.Append(delimiter);
            var keyValuePair = iter.Current;
            sb.Append(keyValuePair.Key);
            sb.Append('=');
            sb.Append(keyValuePair.Value.ToJoinedString<IEnumerable<string>, string>());
        }

        sb.Append(suffix);
        return sb.ToString();
    }
    
}