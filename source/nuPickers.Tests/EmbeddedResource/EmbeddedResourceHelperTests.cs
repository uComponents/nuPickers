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
            Assert.IsTrue(EmbeddedResourceHelper.ResourceExists("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditor.css"));
            Assert.IsTrue(EmbeddedResourceHelper.ResourceExists("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditor.html"));
            Assert.IsTrue(EmbeddedResourceHelper.ResourceExists("nuPickers.Shared.CheckBoxPicker.CheckBoxPickerEditorController.js"));
        }

        [TestMethod]
        public void GetResourcesThatDoNotExist()
        {
            Assert.IsNull(EmbeddedResourceHelper.GetResource(null));
            Assert.IsNull(EmbeddedResourceHelper.GetResource(string.Empty));
            Assert.IsNull(EmbeddedResourceHelper.GetResource(".nu"));
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
    }
}