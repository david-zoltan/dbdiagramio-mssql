namespace DbDbiagramIo.MsSql
{
    public class ForeignKeyDto
    {
        public string SourceTableName { get; }
        public string SourceColumnName { get; }
        public string DestinationTableName { get; }
        public string DestinationColumnName { get; }

        public ForeignKeyDto( string sourceTableName, string sourceColumnName, string destinationTableName, string destinationColumnName )
        {
            SourceTableName = sourceTableName;
            SourceColumnName = sourceColumnName;
            DestinationTableName = destinationTableName;
            DestinationColumnName = destinationColumnName;
        }

        public string ToDbDiagramDto()
        {
            return $"Ref: {SourceTableName}.{SourceColumnName} > {DestinationTableName}.{DestinationColumnName}";
        }
    }
}
