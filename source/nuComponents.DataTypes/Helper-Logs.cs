using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Logging;

namespace nuComponents.DataTypes
{
    internal static partial class Helper
    {
        internal static class Logs
        {
            internal static void LogExeption<T>(string message, Exception e)
            {
               LogHelper.Error<T>(message,e);
            }
        }
    }
}
