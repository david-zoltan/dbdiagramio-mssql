using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;

namespace DbDbiagramIo.MsSql
{
    public class MsSqlSchemaReader
    {
        private const string ForeignKeysQuery = "SELECT constraints.CONSTRAINT_NAME as constraint_name, column_source.TABLE_NAME as source_table, column_source.COLUMN_NAME as source_column, column_destination.TABLE_NAME as destination_table, column_destination.COLUMN_NAME as destination_column FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS constraints join INFORMATION_SCHEMA.KEY_COLUMN_USAGE column_source on constraints.CONSTRAINT_NAME = column_source.CONSTRAINT_NAME join INFORMATION_SCHEMA.KEY_COLUMN_USAGE column_destination on constraints.UNIQUE_CONSTRAINT_NAME = column_destination.CONSTRAINT_NAME and column_source.ORDINAL_POSITION = column_destination.ORDINAL_POSITION;";
        private const string ColumnsQuery = "select table_name, column_name, ordinal_position, DATA_TYPE from information_schema.COLUMNS;";

        public static (TableDto[], ForeignKeyDto[]) ReadTablesAndForeignKeysFromDb( string connectionString )
        {
            List<ColumnDto> columns = new List<ColumnDto>();
            List<ForeignKeyDto> foreignKeys = new List<ForeignKeyDto>();

            using (SqlConnection connection = new SqlConnection( connectionString ))
            {
                using (SqlCommand cmd = new SqlCommand( ColumnsQuery, connection ))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tableName = (string)reader[ "table_name" ];
                            string name = (string)reader[ "column_name" ];
                            int ordinalPosition = int.Parse( reader[ "ordinal_position" ].ToString(), CultureInfo.InvariantCulture );
                            string sqlType = (string)reader[ "DATA_TYPE" ];

                            ColumnDto columnDto = new ColumnDto()
                            {
                                Name = name,
                                OrdinalPosition = ordinalPosition,
                                SqlType = sqlType,
                                TableName = tableName
                            };

                            columns.Add( columnDto );
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand( ForeignKeysQuery, connection ))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string sourceTableName = (string)reader[ "source_table" ];
                            string sourceColumnName = (string)reader[ "source_column" ];
                            string destinationTableName = (string)reader[ "destination_table" ];
                            string destinationColumnName = (string)reader[ "destination_column" ];

                            ForeignKeyDto fk = new ForeignKeyDto( sourceTableName, sourceColumnName, destinationTableName, destinationColumnName );

                            foreignKeys.Add( fk );
                        }
                    }
                }

            }

            Dictionary<string, TableDto> name2table = new Dictionary<string, TableDto>();
            List<TableDto> tables = new List<TableDto>();
            foreach (ColumnDto column in columns)
            {
                if (!name2table.ContainsKey( column.TableName ))
                    name2table.Add( column.TableName, new TableDto( column.TableName ) );

                TableDto table = name2table[ column.TableName ];
                table.Add( column );
                tables.Add( table );
            }

            return (tables.ToArray(), foreignKeys.ToArray());
        }
    }
}