namespace Nummi.Core.Util; 

public static class Colorize {

    private const string ANSI_RESET = "\u001B[0m";
    private const string ANSI_RED = "\u001B[31m";
    private const string ANSI_YELLOW = "\u001B[33m";
    private const string ANSI_GREEN = "\u001B[32m";
    private const string ANSI_BLUE = "\u001B[34m";
    private const string ANSI_PURPLE = "\u001B[35m";
    private const string ANSI_CYAN = "\u001B[36m";

    public static string Red(this string str) {
        return ANSI_RED + str + ANSI_RESET;
    }

    public static string Green(this string str) {
        return ANSI_GREEN + str + ANSI_RESET;
    }

    public static string Blue(this string str) {
        return ANSI_BLUE + str + ANSI_RESET;
    }

    public static string Cyan(this string str) {
        return ANSI_CYAN + str + ANSI_RESET;
    }

    public static string Purple(this string str) {
        return ANSI_PURPLE + str + ANSI_RESET;
    }

    public static string Yellow(this string str) {
        return ANSI_YELLOW + str + ANSI_RESET;
    }

}