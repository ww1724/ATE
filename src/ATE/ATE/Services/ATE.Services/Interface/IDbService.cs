using System;
using System.Collections.Generic;
using System.Text;

namespace ATE.Service.Interface
{
    public interface IDbService
    {
        List<T> Query<T>(string condition = "");

        int Insert<T>(T entity);

        int Update<T>(T entity);

        int Delete<T>(T entity);
    }
}
