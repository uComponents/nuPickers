
angular.module('umbraco.resources')
    .factory('nuPickers.Shared.SaveFormat.SaveFormatResource',
        function () {

            return {

                // pickedOptions: [{"key":"","label":""},{"key":"","label":""}...]
                // returns a string representation of the picked options as per the configured SaveFormat
                createSaveValue: function (config, pickedOptions) {

                    if (pickedOptions == null || pickedOptions.length == 0 || pickedOptions[0] == null) {
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
                            var xml = '<Picker>';

                            for (var i = 0; i < pickedOptions.length; i++) {
                                xml += '<Picked Key="' + pickedOptions[i].key + '">'
                                xml += '<![CDATA[' + pickedOptions[i].label + ']]>';
                                xml += '</Picked>';
                            }

                            xml += '</Picker>';

                            return xml;
                            break;

                        case 'relationsOnly': // when saving to relations only
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
                        $(xml).find('Picked').each(function () {
                            keys.push($(this).attr('Key'));
                        });

                        return keys;
                    }
                    catch (error) {
                    }

                    return saveValue.split(','); // csv
                },

                // saveValue will be either json or xml, so both key/label can be returned
                getSavedItems: function (saveValue) {
                    if (saveValue instanceof Array) // json
                    {
                        return saveValue;
                    }

                    try {
                        var xml = $.parseXML(saveValue);
                        var items = new Array();
                        $(xml).find('Picked').each(function () {
                            items.push({
                                'key': $(this).attr('Key'),
                                'label': $(this).text()
                            });
                        });

                        return items;
                    }
                    catch (error) {

                    }

                    return null;
                }
            };
        }
);