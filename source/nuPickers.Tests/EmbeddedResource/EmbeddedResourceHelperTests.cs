namespace nuPickers.Tests.EmbeddedResource
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using nuPickers.EmbeddedResource;

    [TestClass]
    public class HelperEmbeddedResourceHelperTests
    {
        [TestMethod]
        public void ResourceNamesThatDoNotExist()
        {
            Assert.IsFalse(EmbeddedResourceHelper.ResourceExists(null));
            Assert.IsFalse(EmbeddedResourceHelper.ResourceExists(string.Empty));
            Assert.IsFalse(EmbeddedResourceHelper.ResourceExists("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditor.css.nu"));
            Assert.IsFalse(EmbeddedResourceHelper.ResourceExists("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditorController.js.nu"));
        }

        [TestMethod]
        public void ResourceNamesThatExist()
        {
            Assert.IsTrue(EmbeddedResourceHelper.ResourceExists("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditor.css"));
            Assert.IsTrue(EmbeddedResourceHelper.ResourceExists("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditor.html"));
            Assert.IsTrue(EmbeddedResourceHelper.ResourceExists("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditorController.js"));
        }

        [TestMethod]
        public void GetResourcesThatDoNotExist()
        {
            Assert.IsNull(EmbeddedResourceHelper.GetResource(null));
            Assert.IsNull(EmbeddedResourceHelper.GetResource(string.Empty));
            Assert.IsNull(EmbeddedResourceHelper.GetResource("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditor.css.nu"));
            Assert.IsNull(EmbeddedResourceHelper.GetResource("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditorController.js.nu"));
        }

        [TestMethod]
        public void GetResourcesThatExist()
        {
            Assert.IsNotNull(EmbeddedResourceHelper.GetResource("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditor.css"));
            Assert.IsNotNull(EmbeddedResourceHelper.GetResource("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditor.html"));
            Assert.IsNotNull(EmbeddedResourceHelper.GetResource("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditorController.js"));
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
            Assert.IsNull(EmbeddedResourceHelper.GetResourceNameFromPath("~/folder/file.ext"));
        }

    }
}