namespace nuPickers.EmbeddedResource
{
    internal static class EmbeddedResource
    {
        /// <summary>
        /// Root url for all embedded resources
        /// </summary>
        internal const string ROOT_URL = "~/App_Plugins/nuPickers/Shared/";

        /// <summary>
        /// namespace prefix for all embedded resources
        /// </summary>
        internal const string RESOURCE_PREFIX = "nuPickers.Shared.";

        /// <summary>
        /// Custom extension so ClientDependency framework can delegate handling back to nuPickers for the embedded content
        /// </summary>
        internal const string FILE_EXTENSION = "";
    }
}