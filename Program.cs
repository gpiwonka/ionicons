// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Text.RegularExpressions;


Console.WriteLine("Extracting values:");

WebClient client = new WebClient();
String downloadedString = client.DownloadString("https://ionic.io/ionicons/v2/cheatsheet.html");

Console.WriteLine(downloadedString);
string pattern = @"<i\s+class=""icon\s+(ion-[^""]+)""[^>]*></i>[\s\S]*?<input\s+class=""html""[^>]*\svalue=""([^""]+)""[^>]*>";

Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
MatchCollection matches = regex.Matches(downloadedString);

List<(string IconClass, string HtmlValue)> extractedValues = new List<(string IconClass, string HtmlValue)>();
foreach (Match match in matches)
{
    string iconClass = match.Groups[1].Value;
    string htmlValue = match.Groups[2].Value;
    extractedValues.Add((iconClass, htmlValue));
}

Console.WriteLine("Writing values:");
string filePath = "ionicons.xaml";
using (StreamWriter writer = new StreamWriter(filePath))
{
    writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
    writer.WriteLine("<?xaml-comp compile=\"true\" ?>");
    writer.WriteLine("<ResourceDictionary \r\n    xmlns=\"http://schemas.microsoft.com/dotnet/2021/maui\"\r\n    xmlns:x=\"http://schemas.microsoft.com/winfx/2009/xaml\">\r\n");
    foreach (var value in extractedValues)
    {
        writer.WriteLine($"<x:String x:Key=\"{value.IconClass.Replace("-","_")}\">{value.HtmlValue.Replace("&amp;","&")}</x:String>");
    }

    writer.WriteLine("</ResourceDictionary>");
}

Console.WriteLine("Done");
Console.ReadLine();


