using SqlSugar;

namespace ATE.Service.Entities
{
    [SugarTable("ProductInfo")]
    public class ProductEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(ColumnDataType = "varchar(100)")]
        public string Name { get; set; }

        [SugarColumn(ColumnDataType = "varchar(100)")]
        public string Type { get; set; }

        [SugarColumn(ColumnDataType = "varchar(4000)")]
        public string Description { get; set; }


    }
}
