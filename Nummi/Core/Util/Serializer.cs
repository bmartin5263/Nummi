using System.Globalization;
using CsvHelper;

namespace Nummi.Core.Util; 

public class Serializer {

    public static IEnumerable<T> ReadCsv<T>(string path) {
        var reader = new StreamReader(path);
        return ReadCsv<T>(reader);
    }

    public static IEnumerable<T> ReadCsv<T>(Stream stream) {
        var reader = new StreamReader(stream);
        return ReadCsv<T>(reader);
    }

    public static IEnumerable<T> ReadCsv<T>(StreamReader reader) {
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<T>();
    }
    
}