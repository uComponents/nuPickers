using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Services;

namespace nuPickers.Validators
{
    public class RequiredValidator : IValueRequiredValidator, IManifestValueValidator
    {
        private readonly ILocalizedTextService _textService;

        public RequiredValidator() : this(Current.Services.TextService)
        {
        }

        public RequiredValidator(ILocalizedTextService textService)
        {
            _textService = textService;
        }

        /// <inheritdoc cref="IManifestValueValidator.ValidationName"/>
        public string ValidationName => "Required";

        /// <inheritdoc cref="IValueValidator.Validate"/>
        public IEnumerable<ValidationResult> Validate(object value, string valueType, object dataTypeConfiguration)
        {
            return ValidateRequired(value, valueType);
        }

        /// <inheritdoc cref="IValueRequiredValidator.ValidateRequired"/>
        public IEnumerable<ValidationResult> ValidateRequired(object value, string valueType)
        {
            if (value == null)
            {
                yield return new ValidationResult(_textService.Localize("validation", "invalidNull"), new[] {"value"});
                yield break;
            }

            if (valueType.InvariantEquals(ValueTypes.Json))
            {
                if (DetectIsEmptyJson(value.ToString()))
                    yield return new ValidationResult(_textService.Localize("validation", "invalidEmpty"), new[] { "value" });
                yield break;
            }

            if (value.ToString().IsNullOrWhiteSpace())
            {
                yield return new ValidationResult(_textService.Localize("validation", "invalidEmpty"), new[] { "value" });
            }
        }
        static bool DetectIsEmptyJson( string input)
        {
            return ((IList) JsonEmpties).Contains(Whitespace.Value.Replace(input, string.Empty));
        }
        static readonly string[] JsonEmpties = { "[]", "{}" };
        static readonly Lazy<Regex> Whitespace = new Lazy<Regex>(() => new Regex(@"\s+", RegexOptions.Compiled));
    }
}