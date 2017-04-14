namespace nuPickers.EmbeddedResource
{
    internal static class EmbeddedResource
    {
        internal const string RootUrl = "~/App_Plugins/nuPickers/Shared/";

        /// <summary>
        /// Custom extension so ClientDependency framework can delegate handling back to nuPickers for the embedded content 
        /// </summary>
        internal const string FILE_EXTENSION = ".nu";
    }
}
