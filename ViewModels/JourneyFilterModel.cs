using Microsoft.AspNetCore.Mvc;

namespace Atut.ViewModels
{
    public class JourneyFilterModel
    {
        [FromQuery]
        public string Aa { get; set; }
        [FromQuery]
        public string Bb { get; set; }
    }
}