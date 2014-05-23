
namespace nuComponents.DataTypes.Shared.SqlDataSource
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using umbraco.DataLayer;
    using nuComponents.DataTypes.Shared.Picker;
    using System.Text.RegularExpressions;

    public class SqlDataSource
    {
        public string SqlExpression { get; set; }

        public string ConnectionString { get; set; }

        public string Typeahead { get; set; } // the value supplied by the user - the current typeahead text

        public IEnumerable<PickerEditorOption> GetEditorOptions(int contextId) // supply option typeahead param
        {
            List<PickerEditorOption> pickerEditorOptions = new List<PickerEditorOption>();

            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[this.ConnectionString];            
            if (connectionStringSettings != null)
            {
                using (ISqlHelper sqlHelper = DataLayerHelper.CreateSqlHelper(connectionStringSettings.ConnectionString))
                {
                    string sql = Regex.Replace(this.SqlExpression, "\n|\r", " ");
                    List<IParameter> parameters = new List<IParameter>();

                    if (sql.Contains("@contextId"))
                    {
                        parameters.Add(sqlHelper.CreateParameter("@contextId", contextId));
                    }
                    if (sql.Contains("@typeahead"))
                    {
                        parameters.Add(sqlHelper.CreateParameter("@typeahead", this.Typeahead));
                    }

                    using(IRecordsReader recordsReader = sqlHelper.ExecuteReader(sql, parameters.ToArray()))
                    {
                        if(recordsReader != null)
                        {
                            while(recordsReader.Read())
                            {
                                pickerEditorOptions.Add(
                                    new PickerEditorOption()
                                    {
                                        Key = recordsReader.GetObject("Key").ToString(),
                                        Label = recordsReader.GetObject("Label").ToString()
                                    }
                                );
                            }
                        }
                    }
                }
            }

            return pickerEditorOptions;
        }
    }
}
