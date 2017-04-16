
angular.module('umbraco.resources')
    .factory('nuPickers.Shared.SaveFormat.SaveFormatResource',
        function () {

            return {

                /// returns a string representation of the picked options as per the configured SaveFormat
                /// pickedOptions expected to be an array of { 'key': '', 'label': '' } objects
                createSaveValue: function (config, pickedOptions) {

                    if (pickedOptions == null || pickedOptions.length == 0 || pickedOptions[0] == null) {
                        return null;
                    }

                    switch (config.saveFormat) {

                        case 'csv': // 'key, key...'
                        case 'relationsOnly': // special case - used server-side by relationsOnly mapping event (where this value is then wiped)
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

                        default:
                            return null;
                            break;
                    }
                },

                /// returns an array of strings
                /// saveValue is expected to be a string or an array of { 'key': '', 'label': '' } objects
                getSavedKeys: function (saveValue) {

                    // json save format
                    if (saveValue instanceof Array)
                    {
                        return saveValue.map(function (option) { return option.key }).join().split(',');
                    }

                    // parse string for nested json save format (fix to support the Doc Type Grid Editor package)
                    try {
                        var jsonSaveValue = JSON.parse(saveValue);
                        return jsonSaveValue.map(function (option) { return option.key }).join().split(',');
                    }
                    catch (error) { } // suppress

                    // xml save format
                    try {
                        var xml = $.parseXML(saveValue);
                        var keys = new Array();
                        $(xml).find('Picked').each(function () {
                            keys.push($(this).attr('Key'));
                        });

                        return keys;
                    }
                    catch (error) { } // suppress

                    // csv save format
                    return saveValue.split(',');
                },

                /// returns an array of { 'key': '', 'label': '' } objects
                /// saveValue expected to be either json or xml
                getSavedItems: function (saveValue) {

                    // json save format
                    if (saveValue instanceof Array)
                    {
                        return saveValue;
                    }

                    // parse string for nested json save format (fix to support the Doc Type Grid Editor package)
                    try {
                        var nestedJson = JSON.parse(saveValue);
                        if (nestedJson instanceof Array)
                        {
                            return nestedJson;
                        }
                    }
                    catch (error) { } // suppress

                    // xml save format
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
                    catch (error) { } // suppress

                    // csv save format - doesn't support storing of label data
                    return null;
                }
            };
        }
);