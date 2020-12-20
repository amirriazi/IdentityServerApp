using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SharedLibrary;

namespace IdentityServer.Data
{
    public partial class SqlDatabase 
    {
        public SqlDatabase(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private QueryResult ExecuteQuery(string query, CommandType commandType, SqlParameter[] parameters)
        {
            QueryResult result;
            var dS = new DataSet();
            var sw = Stopwatch.StartNew();
            var returnCodeID = parameters.Length - 1;
            var resultTextID = parameters.Length - 2;
            var spResultCodeID = parameters.Length - 3;
            string param = "";
            if (parameters != null & parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    param += parameters[i].ParameterName + ":" + parameters[i].Value;
                }
                param = "{" + param + "}";
            }
            while (true)
            {
                try
                {
                    //using (var connection = new SqlConnection(Configuration.GetConnectionString("SQLDB")) // "Server=IDEAPAD330-04;Initial Catalog=IdentityServerDB;User ID=amirriazi;Password=66177602;"))
                    using (var connection = new SqlConnection(Configuration.GetConnectionString("SQLDB")))
                    {
                        using (var command = new SqlCommand(query, connection) { CommandTimeout = 60000, CommandType = commandType })
                        {
                            using (var dataAdaptor = new SqlDataAdapter(command))
                            {
                                command.Parameters.AddRange(parameters);
                                connection.Open();
                                dataAdaptor.Fill(dS);
                            }
                            if ((int)parameters[returnCodeID].Value != 1)
                            {
                                result = new QueryResult { ReturnCode = (int)parameters[returnCodeID].Value, Text = (string)parameters[resultTextID].Value };

                                break;
                            }
                            if ((int)parameters[spResultCodeID].Value != 1)
                            {
                                result = new QueryResult { SPCode = (int)parameters[spResultCodeID].Value, ReturnCode = (int)parameters[returnCodeID].Value, Text = (string)parameters[resultTextID].Value };

                                break;
                            }
                            result = new QueryResult { DataSet = dS, SPCode = (int)parameters[spResultCodeID].Value, ReturnCode = (int)parameters[returnCodeID].Value, Text = (string)parameters[resultTextID].Value };

                        }
                    }
                }
                catch (Exception ex)
                {
                    result = new QueryResult { Text = ex.Message.ToString(), ReturnCode = ex.HResult };


                }
                break;
            }
            return result;
        }
        private QueryResult ExecuteStoredProcedure(string storedProcedureName, List<SqlParameter> parameters)
        {
            parameters.Add(new SqlParameter { ParameterName = "@output_status", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });
            parameters.Add(new SqlParameter { ParameterName = "@output_message", SqlDbType = SqlDbType.NVarChar, Size = 4000, Direction = ParameterDirection.Output });
            parameters.Add(new SqlParameter { ParameterName = "@returnvalue", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.ReturnValue });

            return ExecuteQuery(storedProcedureName, CommandType.StoredProcedure, parameters.ToArray());
        }
    }
}
