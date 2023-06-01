using System.Dynamic;

namespace PDF_Data_Extractor
{
    public class FlightDetail
    {
        public dynamic Properties { get; } = new ExpandoObject();
    }
}