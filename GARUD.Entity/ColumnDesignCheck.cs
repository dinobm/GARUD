using System.Security.Policy;

namespace GARUD.Entity
{
    public class ColumnDesignCheck
    {
        public string TableName { get; set; }

        public string SchemaName { get; set; }

        public string ColumnName { get; set; }

        public string PrimaryColumnName { get; set; }

        public string NullableFieldMismatch { get; set; }

        public string MaxSizeMismatch { get; set; }

        public string OctetSizeMismatch { get; set; }

        public string DataTypeMismatch { get; set; }
    }
}
