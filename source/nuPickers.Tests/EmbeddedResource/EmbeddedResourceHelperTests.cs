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
            Assert.IsFalse(EmbeddedResourceHelper.ResourceExists(".nu"));
            Assert.IsFalse(EmbeddedResourceHelper.ResourceExists("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditor.css.nu"));
            Assert.IsFalse(EmbeddedResourceHelper.ResourceExists("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditorController.js.nu"));
        }

        [TestMethod]
        public void ResourceNamesThatExist()
        {
            Assert.IsTrue(EmbeddedResourceHelper.ResourceExists("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerConfig.html"));
            Assert.IsTrue(EmbeddedResourceHelper.ResourceExists("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditor.css"));
            Assert.IsTrue(EmbeddedResourceHelper.ResourceExists("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditor.html"));
            Assert.IsTrue(EmbeddedResourceHelper.ResourceExists("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditorController.js"));
        }
    }
}