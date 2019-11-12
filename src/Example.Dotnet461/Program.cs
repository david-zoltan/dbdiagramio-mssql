using DbDbiagramIo.MsSql;
using System;

namespace Example.Dotnet461
{
    class Program
    {
        // TODO: set your own database connection string here
        private const string __ConnectionString = "Server=(local)\\SQLEXPRESS;Database=master;Integrated Security=true;";

        static void Main( string[] args )
        {
            (TableDto[] tables, ForeignKeyDto[] foreignKeys) = MsSqlSchemaReader.ReadTablesAndForeignKeysFromDb( __ConnectionString );

            foreach (TableDto table in tables)
            {
                Console.WriteLine( table.ToDbDbiagramCode() );
            }

            foreach (ForeignKeyDto fk in foreignKeys)
            {
                Console.WriteLine( fk.ToDbDiagramDto() );
            }
        }
    }
}
