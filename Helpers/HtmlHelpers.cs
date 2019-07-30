using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Atut.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlContent ToJson(this IHtmlHelper helper, object obj)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            settings.Converters.Add(new JavaScriptDateTimeConverter());
            return helper.Raw(JsonConvert.SerializeObject(obj, settings));
        }

        public static IHtmlContent UrlWithVersion(this IHtmlHelper helper, string fileEntry, IHostingEnvironment hostingEnvironment)
        {
            var path = $"{hostingEnvironment.WebRootPath}{fileEntry.Replace("/", "\\")}";
            var lastModificationDate = File.GetLastWriteTime(path).ToString("yyyyMMddhhmmss");

            return helper.Raw($"{fileEntry}?v={lastModificationDate}");
        }
    }
}
