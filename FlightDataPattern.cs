using PDF_Data_Extractor;
using System.Text.RegularExpressions;

public class FlightDataPattern
{
    public string Pattern { get; }
    public string PropertyName { get; }

    public FlightDataPattern(string pattern, string propertyName)
    {
        Pattern = $"{pattern}\\s+(\\w+)";
        PropertyName = propertyName;
    }

    public void ExtractData(FlightDetail flightDetail, string input)
    {
        Match match = Regex.Match(input, Pattern);
        if (match.Success)
        {
            var properties = (IDictionary<string, object>)flightDetail.Properties;
            properties[PropertyName] = match.Groups[1].Value.Trim();
        }
    }
}