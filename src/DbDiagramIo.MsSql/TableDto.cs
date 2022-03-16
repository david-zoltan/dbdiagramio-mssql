using System.Collections.Generic;
using System.Text;

namespace DbDbiagramIo.MsSql
{
    public class TableDto
    {
        public string Name { get; }

        private readonly SortedList<int, ColumnDto> columns = new SortedList<int, ColumnDto>();

        internal void Add( ColumnDto column )
        {
            this.columns.Add( column.OrdinalPosition, column );
        }

        public TableDto( string name )
        {
            Name = name;
        }

        public string ToDbDbiagramCode()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine( $"Table \"{Name}\" {{" );

            foreach (ColumnDto column in this.columns.Values)
            {
                stringBuilder.AppendLine( $"  {column.ToDbDiagramCode()}" );
            }

            stringBuilder.AppendLine( "}" );

            return stringBuilder.ToString();
        }
    }
}
