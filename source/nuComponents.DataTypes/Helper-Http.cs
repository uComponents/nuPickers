using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace nuComponents.DataTypes
{
    internal static partial class Helper
    {
        internal static class Http
        {
            /// <summary>
            /// Downloads a url resource and returns it as a string. Maybe move this into a helpers class?
            /// </summary>
            /// <param name="url">URL to download the resource from, may begin with ~/ to get from the local file</param>
            /// <returns>the string based result of the webcall</returns>
            internal static string GetContents(string url)
            {
                using (WebClient client = new WebClient())
                {
                    if (url.StartsWith("~/"))
                    {
                        url = HttpContext.Current.Server.MapPath(url);
                    }

                    return client.DownloadString(url);
                }
            }
        }
    }
}
