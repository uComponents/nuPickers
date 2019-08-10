

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
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
    public class SaveFormatPropertyValueEditor  : IDataValueEditor
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
        public SaveFormatPropertyValueEditor() // for tests, and manifest
        {
            ValueType = ValueTypes.String;
            Validators = new List<IValueValidator>();
        }

        /// <summary>
        /// when saving to the xml cache, if the value can be converted to xml then ensure it's not wrapped in CData
        /// </summary>
        /// <param name="property"></param>
        /// <param name="propertyType"></param>
        /// <param name="dataTypeService"></param>
        /// <returns></returns>
        public override XNode ConvertDbToXml(Property property, PropertyType propertyType, IDataTypeService dataTypeService)
        {
            string value = this.ConvertDbToString(property, propertyType, dataTypeService);

            try
            {
                return XElement.Parse(value);
            }
            catch
            {
                return new XCData(value);
            }
        }

        public IEnumerable<ValidationResult> Validate(object value, bool required, string format)
        {
            List<ValidationResult> results = null;
            var r = Validators.SelectMany(v => v.Validate(value, ValueType, Configuration)).ToList();
            if (r.Any()) { results = r; }

            // mandatory and regex validators cannot be part of valueEditor.Validators because they
            // depend on values that are not part of the configuration, .Mandatory and .ValidationRegEx,
            // so they have to be explicitly invoked here.

            if (required)
            {
                r = RequiredValidator.ValidateRequired(value, ValueType).ToList();
                if (r.Any()) { if (results == null) results = r; else results.AddRange(r); }
            }

            var stringValue = value?.ToString();
            if (!string.IsNullOrWhiteSpace(format) && !string.IsNullOrWhiteSpace(stringValue))
            {
                r = FormatValidator.ValidateFormat(value, ValueType, format).ToList();
                if (r.Any()) { if (results == null) results = r; else results.AddRange(r); }
            }

            return results ?? Enumerable.Empty<ValidationResult>();
        }

        /// <summary>
        /// A collection of validators for the pre value editor
        /// </summary>
        [JsonProperty("validation")]
        public List<IValueValidator> Validators { get; private set; }

        /// <summary>
        /// Gets the validator used to validate the special property type -level "required".
        /// </summary>
        public virtual IValueRequiredValidator RequiredValidator => new Requiredv();

        /// <summary>
        /// Gets the validator used to validate the special property type -level "format".
        /// </summary>
        public virtual IValueFormatValidator FormatValidator => new RegexValidator();

        public object FromEditor(ContentPropertyData editorValue, object currentValue)
        {
            throw new System.NotImplementedException();
        }

        public object ToEditor(Property property, IDataTypeService dataTypeService, string culture = null, string segment = null)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<XElement> ConvertDbToXml(Property property, IDataTypeService dataTypeService,
            ILocalizationService localizationService, bool published)
        {
            throw new System.NotImplementedException();
        }

        public XNode ConvertDbToXml(PropertyType propertyType, object value, IDataTypeService dataTypeService)
        {
            throw new System.NotImplementedException();
        }

        public string ConvertDbToString(PropertyType propertyType, object value, IDataTypeService dataTypeService)
        {
            throw new System.NotImplementedException();
        }

        [JsonProperty("view", Required = Required.Always)]
        public string View
        {
            get => _view;
            set => _view = IOHelper.ResolveVirtualUrl(value);
        }
        public string ValueType { get; set; }
        public bool IsReadOnly { get; }
        public bool HideLabel { get; }
    }
}