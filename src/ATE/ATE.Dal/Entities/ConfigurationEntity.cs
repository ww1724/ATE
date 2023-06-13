﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Service.Entities
{
    [SugarTable("Configuration")]
    public class ConfigurationEntity
    {

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(ColumnDataType = "Varchar(100)")]
        public string Key { get; set; }

        [SugarColumn(ColumnDataType = "Text")]
        public string Value { get; set; }

        [SugarColumn(ColumnDataType = "Text", IsNullable = true)]
        public string Description { get; set; }
    }
}
