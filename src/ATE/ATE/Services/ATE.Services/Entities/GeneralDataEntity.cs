using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Service.Entities
{
    [SugarTable("GeneralData")]
    public class GeneralDataEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string Key { get; set; }

        [SugarColumn(ColumnDataType = "Text")]
        public string Data { get; set; }

    }
}
