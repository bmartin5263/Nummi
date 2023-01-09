namespace Nummi.Core.Util; 

public static class Files {

    private static readonly string RESOURCES_PATH = $"{Directory.GetCurrentDirectory()}/Resources/";

    public static StreamReader OpenStream(string path) {
        return new StreamReader(RESOURCES_PATH + path);
    }
    
}