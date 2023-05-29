using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Service.Entities
{
    public class UserEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(ColumnDataType = "varchar(100)")]
        public string Name { get; set; }

        [SugarColumn(ColumnDataType = "varchar(100)")]
        public string Role { get; set; }

        [SugarColumn(ColumnDataType = "varchar(30)")]
        public string Passwd { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Email { get; set; }

    }
}
