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

    public static string JoinToString<T>(this IEnumerable<T> enumerable, string delimiter = ", ", string prefix = "", string suffix = "") {
        var builder = new StringBuilder();
        builder.Append(prefix);
        using var iter = enumerable.GetEnumerator();
        if (!iter.MoveNext()) {
            builder.Append(suffix);
            return builder.ToString();
        }

        var value = iter.Current;
        builder.Append(value);
        while (iter.MoveNext()) {
            builder.Append(delimiter);
            value = iter.Current;
            builder.Append(value);
        }

        builder.Append(suffix);
        return builder.ToString();
    }
    
    public static string JoinToString<T>(this IEnumerable<T> enumerable, Func<T, string> mapper, string delimiter = ", ", string prefix = "", string suffix = "") {
        var builder = new StringBuilder();
        builder.Append(prefix);
        using var iter = enumerable.GetEnumerator();
        if (!iter.MoveNext()) {
            builder.Append(suffix);
            return builder.ToString();
        }

        var value = iter.Current;
        builder.Append(mapper(value));
        while (iter.MoveNext()) {
            builder.Append(delimiter);
            value = iter.Current;
            builder.Append(mapper(value));
        }

        builder.Append(suffix);
        return builder.ToString();
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