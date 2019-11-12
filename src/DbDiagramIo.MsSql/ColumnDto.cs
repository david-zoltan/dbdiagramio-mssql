namespace DbDbiagramIo.MsSql
{
    internal class ColumnDto
    {
        public string TableName;
        public string Name;
        public int OrdinalPosition;
        public string SqlType;

        private string DbDiagramType
        {
            get
            {
                string sqlTypeLowerCase = this.SqlType.ToLowerInvariant();

                switch (sqlTypeLowerCase)
                {
                    case "nvarchar":
                        return "varchar";
                    case "datetime2":
                        return "datetime";
                    default:
                        return sqlTypeLowerCase;
                }
            }
        }

        public string ToDbDiagramCode()
        {
            return $"{this.Name} {DbDiagramType}";
        }
    }
}
