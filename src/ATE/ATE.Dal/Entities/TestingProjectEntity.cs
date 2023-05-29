using SqlSugar;
namespace ATE.Services.Entities
{
    public class TestingProjectEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(IsNullable = false)]
        public string ProjectId { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Name { get; set; }

        [SugarColumn(ColumnDataType = "Text")]
        public string Title { get; set; }

        [SugarColumn(ColumnDataType = "Text")]
        public string ClassType { get;set; }

        [SugarColumn(ColumnDataType = "TEXT", IsNullable = true)]
        public string Steps { get; set; }

        [SugarColumn(ColumnDataType = "TEXT", IsNullable = true)]
        public string WorkflowCode { get; set; }
    }
}
