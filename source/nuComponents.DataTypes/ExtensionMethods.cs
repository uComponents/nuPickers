// only here temporarily
namespace nuComponents.DataTypes
{
    using System.IO;
    using System.Text;
    using System.Web.UI;

    internal static class ExtensionMethods
    {
        public static string RenderToString(this Control ctrl)
        {
            var sb = new StringBuilder();

            using (var tw = new StringWriter(sb))
            using (var hw = new HtmlTextWriter(tw))
            {
                ctrl.RenderControl(hw);
            }

            return sb.ToString();
        }
    }
}
