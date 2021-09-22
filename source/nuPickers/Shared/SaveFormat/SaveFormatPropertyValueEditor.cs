using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using nuPickers.Validators;
using Umbraco.Core.Composing.CompositionExtensions;
using Umbraco.Core.IO;
using Umbraco.Core.Models.Editors;
using Umbraco.Core.PropertyEditors;

namespace nuPickers.Shared.SaveFormat
{
    using System.Xml.Linq;
    using Umbraco.Core.Models;
    using Umbraco.Core.Services;

    /// <summary>
    /// This class exists so as to be able to save xml direclty into the Umbraco xml cache
    /// </summary>
    public class SaveFormatPropertyValueEditor : DataValueEditor
    {
        private string _view;

        /// <summary>
        /// reconstruct values from original (default) property value editor
        /// </summary>
        /// <param name="propertyValueEditor"></param>
        public SaveFormatPropertyValueEditor(DataEditorAttribute propertyValueEditor) : this()
        {
            this.HideLabel = propertyValueEditor.HideLabel;
            this.View = propertyValueEditor.View;
            this.ValueType = propertyValueEditor.ValueType;
        }

        public SaveFormatPropertyValueEditor()  // for tests, and manifest
        {
        }


    }
}