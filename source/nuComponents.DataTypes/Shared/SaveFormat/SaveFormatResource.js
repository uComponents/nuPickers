
angular.module('umbraco.resources')
    .factory('nuComponents.DataTypes.Shared.SaveFormat.SaveFormatResource',
        function () {

            return {

                // pickedOptions: [{"key":"","label":""},{"key":"","label":""}...]
                // returns a string representation of the picked options as per the configured SaveFormat
                createSaveValue: function (config, pickedOptions) {
                    
                    if (pickedOptions == null || pickedOptions.length == 0) {
                        return null;
                    }

                    switch (config.saveFormat) {

                        case 'csv': // "key, key..."                        
                            return pickedOptions.map(function (option) { return option.key; }).join();
                            break;

                        case 'json': // some pickers add extra data to the options, so only return a key/label collection
                            return JSON.stringify(
                                            pickedOptions.map(function (option) {
                                                return { 'key': option.key, 'label': option.label }
                                            }));
                            break;

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

                        case 'none': // when saving to relations only
                        default: 
                            return null;
                            break;
                    }
                },

                getSavedKeys: function (saveValue) {

                    if (saveValue instanceof Array) // json
                    {
                        return saveValue.map(function (option) { return option.key }).join().split(',');
                    }

                    try {
                        var xml = $.parseXML(saveValue);
                        var keys = new Array();
                        $(xml).find('PickedOption').each(function () {
                            keys.push($(this).attr('Key'));
                        });

                        return keys;
                    }
                    catch (error) {
                    }

                    return saveValue.split(','); // csv
                }
            };
        }
);