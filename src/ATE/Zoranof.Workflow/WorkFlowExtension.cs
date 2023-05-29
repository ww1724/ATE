using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Persistence.EntityFramework.Interfaces;

namespace Zoranof.WorkFlow
{
    public static class WorkFlowExtension
    {
        private static string DbConnectionString => ConfigurationManager.ConnectionStrings["MysqlConnectionString"].ConnectionString;

        public static IServiceCollection AddCustomWorkFlow(this IServiceCollection services)
        {
            services.AddLogging();
            services.AddWorkflow(x => { 
                x.UseMySQL(DbConnectionString, true, true);
            });

            // 注册steps workflows events repositories
            IWorkflowRepository workflowRepository;
            IEventRepository eventRepository;
            return services;
        }

        public static void AddWorkflowAssembly(this IServiceCollection services, string path) 
        {

        }
    }
}
