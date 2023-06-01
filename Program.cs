using PDF_Data_Extractor;

List<FlightDataPattern> patterns = new()
{
    new FlightDataPattern("From:", "From"),
    new FlightDataPattern("To:", "To"),
    new FlightDataPattern("ALTN1:", "Alternate1"),
    // Add more patterns as needed
};

string filePath = GetFileLocation();

// Create FlightDataExtractor instance
FlightDataExtractor extractor = new FlightDataExtractor(patterns);

// Extract flight data
List<FlightDetail> extractedData = extractor.ExtractFlightData(filePath);

// Access extracted data
foreach (FlightDetail flightDetail in extractedData)
{
    Console.WriteLine($"From: {flightDetail.Properties.From}");
    Console.WriteLine($"To: {flightDetail.Properties.To}");
    Console.WriteLine($"Alternate1: {flightDetail.Properties.Alternate1}");
    Console.WriteLine();

    // Access other dynamically added properties
    // For example: Console.WriteLine($"Alternate1: {flightDetail.Properties.Alternate1}");
}


static string GetFileLocation()
{
    string baseDirectory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)!;
    string rootpath = Directory.GetParent(Directory.GetParent(Directory.GetParent(baseDirectory)!.FullName)!.FullName)!.FullName;
    var fileLocation = Path.Combine(rootpath, "FlightGroup.pdf");
    return fileLocation;
}



