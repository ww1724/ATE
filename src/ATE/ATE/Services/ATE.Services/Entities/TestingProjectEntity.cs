using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Services.Entities
{
    public class TestingProjectEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(ColumnDataType = "TEXT")]
        public string Steps { get; set; }

        [SugarColumn(ColumnDataType = "TEXT")]
        public string WorkflowCode { get; set; }
    }
}
