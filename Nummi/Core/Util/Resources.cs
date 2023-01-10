namespace Nummi.Core.Util;

public static class Resources {

    public static string BitStampBtcBch1H { get; } = "/Bitstamp_BCHBTC_1h.csv";
    public static string BitStampBtcUsd1H { get; } = "/Bitstamp_BTCUSD_1h.csv";

    private static readonly string RESOURCES_PATH = $"{Directory.GetCurrentDirectory()}/Resources/";

    public static StreamReader OpenStream(string path) {
        return new StreamReader(RESOURCES_PATH + path);
    }
    
}