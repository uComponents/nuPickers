using nuPickers.EmbeddedResource;

namespace nuPickers.Tests.EmbeddedResource
{
    using NUnit.Framework;
    public class HelperEmbeddedResourceHelperTests
    {
        // reference to known embedded resources (picked first folder in shared)
        private const string HtmlResource = "CheckBoxPicker.CheckBoxPickerEditor.html";
        private const string CssResource = "CheckBoxPicker.CheckBoxPickerEditor.css";
        private const string JsResource = "CheckBoxPicker.CheckBoxPickerEditorController.js";

        [Test]
        public void ResourceNamesThatDoNotExist()
        {
            Assert.IsFalse(EmbeddedResourceHelper.ResourceExists(null));
            Assert.IsFalse(EmbeddedResourceHelper.ResourceExists(string.Empty));
            // file extension is for client dependency framework, so used not needed for html files
            Assert.IsFalse(EmbeddedResourceHelper.ResourceExists(nuPickers.EmbeddedResource.EmbeddedResource.RESOURCE_PREFIX + CssResource + nuPickers.EmbeddedResource.EmbeddedResource.FILE_EXTENSION));
            Assert.IsFalse(EmbeddedResourceHelper.ResourceExists(nuPickers.EmbeddedResource.EmbeddedResource.RESOURCE_PREFIX + JsResource + nuPickers.EmbeddedResource.EmbeddedResource.FILE_EXTENSION));
        }

        [Test]
        public void ResourceNamesThatExist()
        {
            Assert.IsTrue(EmbeddedResourceHelper.ResourceExists(nuPickers.EmbeddedResource.EmbeddedResource.RESOURCE_PREFIX + HtmlResource));
            Assert.IsTrue(EmbeddedResourceHelper.ResourceExists(nuPickers.EmbeddedResource.EmbeddedResource.RESOURCE_PREFIX + CssResource));
            Assert.IsTrue(EmbeddedResourceHelper.ResourceExists(nuPickers.EmbeddedResource.EmbeddedResource.RESOURCE_PREFIX + JsResource));
        }

        [Test]
        public void GetResourcesThatDoNotExist()
        {
            Assert.IsNull(EmbeddedResourceHelper.GetResource(null));
            Assert.IsNull(EmbeddedResourceHelper.GetResource(string.Empty));
            // file extension is for client dependency framework, so used not needed for html files
            Assert.IsNull(EmbeddedResourceHelper.GetResource(nuPickers.EmbeddedResource.EmbeddedResource.RESOURCE_PREFIX + CssResource + nuPickers.EmbeddedResource.EmbeddedResource.FILE_EXTENSION));
            Assert.IsNull(EmbeddedResourceHelper.GetResource(nuPickers.EmbeddedResource.EmbeddedResource.RESOURCE_PREFIX + JsResource + nuPickers.EmbeddedResource.EmbeddedResource.FILE_EXTENSION));
        }

        [Test]
        public void GetResourcesThatExist()
        {
            Assert.IsNotNull(EmbeddedResourceHelper.GetResource(nuPickers.EmbeddedResource.EmbeddedResource.RESOURCE_PREFIX + HtmlResource));
            Assert.IsNotNull(EmbeddedResourceHelper.GetResource(nuPickers.EmbeddedResource.EmbeddedResource.RESOURCE_PREFIX + CssResource));
            Assert.IsNotNull(EmbeddedResourceHelper.GetResource(nuPickers.EmbeddedResource.EmbeddedResource.RESOURCE_PREFIX + JsResource));
        }

        [Test]
        public void GetResourceNameFromValidPath()
        {
            Assert.AreEqual(
                    nuPickers.EmbeddedResource.EmbeddedResource.RESOURCE_PREFIX,
                    EmbeddedResourceHelper.GetResourceNameFromPath(nuPickers.EmbeddedResource.EmbeddedResource.ROOT_URL));

            Assert.AreEqual(
                    nuPickers.EmbeddedResource.EmbeddedResource.RESOURCE_PREFIX + "folder.file.ext",
                    EmbeddedResourceHelper.GetResourceNameFromPath(nuPickers.EmbeddedResource.EmbeddedResource.ROOT_URL + "folder/file.ext"));

            Assert.AreEqual(
                    nuPickers.EmbeddedResource.EmbeddedResource.RESOURCE_PREFIX + "folder.file.ext",
                    EmbeddedResourceHelper.GetResourceNameFromPath(nuPickers.EmbeddedResource.EmbeddedResource.ROOT_URL + "folder/file.ext" + nuPickers.EmbeddedResource.EmbeddedResource.FILE_EXTENSION));
        }

        [Test]
        public void GetResourceNameFromInvalidPath()
        {
            Assert.IsNull(EmbeddedResourceHelper.GetResourceNameFromPath(null));
            Assert.IsNull(EmbeddedResourceHelper.GetResourceNameFromPath(string.Empty));
            Assert.IsNull(EmbeddedResourceHelper.GetResourceNameFromPath("folder/file.ext"));
            Assert.IsNull(EmbeddedResourceHelper.GetResourceNameFromPath("/folder/file.ext"));
            Assert.IsNull(EmbeddedResourceHelper.GetResourceNameFromPath("~/folder/file.ext"));
            Assert.IsNull(EmbeddedResourceHelper.GetResourceNameFromPath("~/folder/file.ext" + nuPickers.EmbeddedResource.EmbeddedResource.FILE_EXTENSION));
        }
    }
}