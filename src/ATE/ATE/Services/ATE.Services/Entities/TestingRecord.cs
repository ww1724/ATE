using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Service.Entities
{
    public class TestingRecord
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string TestingSteps { get; set; }

        // foreign key
        public int TestingCode { get; set; }

        public int ProductId { get; set; }
    }
}
