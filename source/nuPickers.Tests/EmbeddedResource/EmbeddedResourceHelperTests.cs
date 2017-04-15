namespace nuPickers.Tests.EmbeddedResource
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using nuPickers.EmbeddedResource;

    [TestClass]
    public class HelperEmbeddedResourceHelperTests
    {
        // reference to known embedded resources (picked first folder in shared)
        private const string HTML_RESOURCE = "CheckBoxPicker.CheckBoxPickerEditor.html";
        private const string CSS_RESOURCE = "CheckBoxPicker.CheckBoxPickerEditor.css";
        private const string JS_RESOURCE = "CheckBoxPicker.CheckBoxPickerEditorController.js";

        [TestMethod]
        public void ResourceNamesThatDoNotExist()
        {
            Assert.IsFalse(EmbeddedResourceHelper.ResourceExists(null));
            Assert.IsFalse(EmbeddedResourceHelper.ResourceExists(string.Empty));
            // file extension is for client dependency framework, so used not needed for html files
            Assert.IsFalse(EmbeddedResourceHelper.ResourceExists(EmbeddedResource.RESOURCE_PREFIX + CSS_RESOURCE + EmbeddedResource.FILE_EXTENSION));
            Assert.IsFalse(EmbeddedResourceHelper.ResourceExists(EmbeddedResource.RESOURCE_PREFIX + JS_RESOURCE + EmbeddedResource.FILE_EXTENSION));
        }

        [TestMethod]
        public void ResourceNamesThatExist()
        {
            Assert.IsTrue(EmbeddedResourceHelper.ResourceExists(EmbeddedResource.RESOURCE_PREFIX + HTML_RESOURCE));
            Assert.IsTrue(EmbeddedResourceHelper.ResourceExists(EmbeddedResource.RESOURCE_PREFIX + CSS_RESOURCE));
            Assert.IsTrue(EmbeddedResourceHelper.ResourceExists(EmbeddedResource.RESOURCE_PREFIX + JS_RESOURCE));
        }

        [TestMethod]
        public void GetResourcesThatDoNotExist()
        {
            Assert.IsNull(EmbeddedResourceHelper.GetResource(null));
            Assert.IsNull(EmbeddedResourceHelper.GetResource(string.Empty));
            // file extension is for client dependency framework, so used not needed for html files
            Assert.IsNull(EmbeddedResourceHelper.GetResource(EmbeddedResource.RESOURCE_PREFIX + CSS_RESOURCE + EmbeddedResource.FILE_EXTENSION));
            Assert.IsNull(EmbeddedResourceHelper.GetResource(EmbeddedResource.RESOURCE_PREFIX + JS_RESOURCE + EmbeddedResource.FILE_EXTENSION));
        }

        [TestMethod]
        public void GetResourcesThatExist()
        {
            Assert.IsNotNull(EmbeddedResourceHelper.GetResource(EmbeddedResource.RESOURCE_PREFIX + HTML_RESOURCE));
            Assert.IsNotNull(EmbeddedResourceHelper.GetResource(EmbeddedResource.RESOURCE_PREFIX + CSS_RESOURCE));
            Assert.IsNotNull(EmbeddedResourceHelper.GetResource(EmbeddedResource.RESOURCE_PREFIX + JS_RESOURCE));
        }

        [TestMethod]
        public void GetResourceNameFromValidPath()
        {
            Assert.AreEqual(
                    EmbeddedResource.RESOURCE_PREFIX,
                    EmbeddedResourceHelper.GetResourceNameFromPath(EmbeddedResource.ROOT_URL));

            Assert.AreEqual(
                    EmbeddedResource.RESOURCE_PREFIX + "folder.file.ext",
                    EmbeddedResourceHelper.GetResourceNameFromPath(EmbeddedResource.ROOT_URL + "folder/file.ext"));

            Assert.AreEqual(
                    EmbeddedResource.RESOURCE_PREFIX + "folder.file.ext",
                    EmbeddedResourceHelper.GetResourceNameFromPath(EmbeddedResource.ROOT_URL + "folder/file.ext" + EmbeddedResource.FILE_EXTENSION));
        }

        [TestMethod]
        public void GetResourceNameFromInvalidPath()
        {
            Assert.IsNull(EmbeddedResourceHelper.GetResourceNameFromPath(null));
            Assert.IsNull(EmbeddedResourceHelper.GetResourceNameFromPath(string.Empty));
            Assert.IsNull(EmbeddedResourceHelper.GetResourceNameFromPath("folder/file.ext"));
            Assert.IsNull(EmbeddedResourceHelper.GetResourceNameFromPath("/folder/file.ext"));
            Assert.IsNull(EmbeddedResourceHelper.GetResourceNameFromPath("~/folder/file.ext"));
            Assert.IsNull(EmbeddedResourceHelper.GetResourceNameFromPath("~/folder/file.ext" + EmbeddedResource.FILE_EXTENSION));
        }
    }
}