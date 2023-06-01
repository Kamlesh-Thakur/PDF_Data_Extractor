using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using PDF_Data_Extractor;
using System.Text.RegularExpressions;

public class FlightDataExtractor
{
    private readonly List<FlightDataPattern> patterns;

    public FlightDataExtractor(List<FlightDataPattern> patterns)
    {
        this.patterns = patterns;
    }

    public List<FlightDetail> ExtractFlightData(string filePath)
    {
        List<FlightDetail> extractedData = new();

        using (PdfDocument pdfDoc = new(new PdfReader(filePath)))
        {
            FlightDetail? currentFlightDetail = null;
            bool newFlightDetail = true;

            for (int pageNumber = 1; pageNumber <= pdfDoc.GetNumberOfPages(); pageNumber++)
            {
                PdfPage page = pdfDoc.GetPage(pageNumber);
                ITextExtractionStrategy strategy = new LocationTextExtractionStrategy();
                string pageText = PdfTextExtractor.GetTextFromPage(page, strategy);

                string currentPageHeader = GetPageHeader(pageText);

                if (currentPageHeader == "Page 1")
                {
                    if (currentFlightDetail != null)
                    {
                        extractedData.Add(currentFlightDetail);
                    }

                    currentFlightDetail = new FlightDetail();
                    newFlightDetail = true;
                }

                if (currentFlightDetail != null && newFlightDetail)
                {
                    foreach (FlightDataPattern pattern in patterns)
                    {
                        pattern.ExtractData(currentFlightDetail, pageText);
                    }

                    if (AreAllPropertiesSet(currentFlightDetail))
                    {
                        newFlightDetail = false;
                    }
                }
            }

            if (currentFlightDetail != null)
            {
                extractedData.Add(currentFlightDetail);
            }
        }

        return extractedData;
    }


    private static string GetPageHeader(string pageText)
    {
        string headerPattern = @"\bPage\s+\s*1\b";
        Match match = Regex.Match(pageText, headerPattern, RegexOptions.IgnoreCase);
        if (match.Success)
        {
            string currentPageHeader = match.Value.Trim();
            currentPageHeader = Regex.Replace(currentPageHeader, @"\s+", " "); // Remove extra whitespaces
            return currentPageHeader;
        }
        return string.Empty;
    }

    private bool AreAllPropertiesSet(FlightDetail flightDetail)
    {
        var properties = (IDictionary<string, object>)flightDetail.Properties;
        return patterns.All(pattern => properties.ContainsKey(pattern.PropertyName));
    }
}
