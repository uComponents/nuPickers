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
            Assert.IsFalse(EmbeddedResourceHelper.ResourceExists("nuPickers.Shared.Labels.LabelsEditorController.js.nu"));
        }

        [TestMethod]
        public void ResourceNamesThatExist()
        {
            Assert.IsTrue(EmbeddedResourceHelper.ResourceExists("nuPickers.Shared.Labels.LabelsEditorController.js"));
        }
    }
}
