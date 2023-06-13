using ATE.Service.Interface;
using ATE.Services.Entities;
using ATE.Share;
using SqlSugar;
using System.Collections.Generic;
using System.Configuration;
using ATE.Service.Entities;

namespace ATE.Service
{
    public class DbService : IDbService, IServiceBase
    {
        public string Name => "DbService";

        public string Description => "Provide DbService of Sqlite";

        public ServiceState ServiceState { get; set; } = ServiceState.Uninit;

        private string DbConnectionString => ConfigurationManager.ConnectionStrings["MysqlConnectionString"].ConnectionString;

        private DbType DbType => DbType.MySql;

        private SqlSugarClient sugarClient { get; set; }
 
        public DbService()
        {
            sugarClient = new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = DbConnectionString,
                DbType = DbType
            });
            InitTables();
            ServiceState = ServiceState.Running;
        }

        private void InitTables()
        {
            sugarClient.DbMaintenance.CreateDatabase();
            sugarClient.CodeFirst.InitTables(
                typeof(ProductEntity),
                typeof(UserEntity),
                typeof(TestingCodeEntity),
                typeof(TestingProjectEntity),
                typeof(ConfigurationEntity));
            if (!sugarClient.Queryable<UserEntity>().Any())
            {
                sugarClient.InsertableByObject(new UserEntity
                {
                    Name = "Zoran.Yang",
                    Role = "SuperAdmin",
                    Passwd = "db49172",
                }).ExecuteCommand();
            }

            if (!sugarClient.Queryable<TestingProjectEntity>().Any())
            {
                sugarClient.InsertableByObject(new TestingProjectEntity
                {
                    Name="defautl",
                    ClassType = "null",
                    ProjectId = "-1",
                    Steps = "",
                    Title ="数据库初始化测试",
                     WorkflowCode = ""
                }).ExecuteCommand();
            }

            if (!sugarClient.Queryable<ConfigurationEntity>().Any())
            {
                sugarClient.InsertableByObject(new ConfigurationEntity
                {
                    Key="IsInitializeDb",
                    Value="True"
                }).ExecuteCommand();
            }
        }

        /// <summary>
        /// 通用查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public List<T> Query<T>(string condition = "")
        {
            return sugarClient.Queryable<T>().ToList();
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        public int Insert<T>(T entity)
        {
            return sugarClient.InsertableByObject(entity).ExecuteCommand();
        }


        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Update<T>(T entity)
        {
            return 0;
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Delete<T>(T entity)
        {
            return 0;
        }
    }
}
