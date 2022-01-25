using System.Text.RegularExpressions;

public static class Utils 
{
    public static string RemoveSpecialChars(string input) 
    {
        var rgx = new Regex("[^a-zA-Z0-9 ]");
        var str = rgx.Replace(input, "");

        return str;
    }
}
