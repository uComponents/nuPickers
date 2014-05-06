namespace nuComponents.DataTypes.Interfaces
{
    /// <summary>
    /// this interface is to ensure that Umbraco will serialize these propertes into the $scope.model.config 
    /// (although an attribute parameter could cause it to be serialized differently)
    /// </summary>
    internal interface IPickerPreValueEditor
    {
        /// <summary>
        /// this is the angular view to configure the data source properties
        /// </summary>
        string DataSource { get; set; }

        /// <summary>
        /// every picker must define an api controller from which it's editor will get it's options [{key:markup}...]
        /// </summary>
        string ApiController { get; set; }
    }
}
