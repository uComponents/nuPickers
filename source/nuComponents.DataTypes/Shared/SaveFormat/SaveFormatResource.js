
angular.module('umbraco.resources')
    .factory('nuComponents.DataTypes.Shared.SaveFormat.SaveFormatResource',
        function () {

            return {

                // pickedOptions: [{"key":"","label":""},{"key":"","label":""}...]
                createSaveValue: function (config, pickedOptions) {

                    switch (config.saveFormat) {

                        case 'csv': // "key, key..."                        
                        default:
                            return pickedOptions.map(function (option) { return option.key; }).join();
                            break;

                            //case 'json': // [{"key":"","label":""},{"key":"","label":""}...] (some pickers add extra data to the options)                            
                            //    return pickedOptions.map(function (opion) { return { key: option.key, label: option.label } });
                            //    break;

                        case 'xml':
                            var xml = '<PickedOptions>';

                            for (var i = 0; i < pickedOptions.length; i++) {
                                xml += '<PickedOption Key="' + pickedOptions[i].key + '">'
                                xml += '<![CDATA[' + pickedOptions[i].label + ']]>';
                                xml += '</PickedOption>';
                            }

                            xml += '</PickedOptions>';

                            return xml;
                            break;

                            //case 'none': // when saving to relations only
                            //    return null;
                            //    break;
                    }
                },

                getSavedKeys: function (saveValue) {

                    // ignore the config and try and decode the save value by looking at the string (incase format is changed)
                    switch (saveValue.charAt(0)) {
                        //case '[':


                        case '<': // TODO: check xml is valid, as a csv key could begin with a '<' !
                            var keys = new Array();
                            var xml = $.parseXML(saveValue); // $ is jQuery

                            $(xml).find('PickedOption').each(function () {
                                keys.push($(this).attr('Key'));
                            });

                            return keys;

                        default:
                            return saveValue.split(','); // csv
                    }
                }
            };
        }
);