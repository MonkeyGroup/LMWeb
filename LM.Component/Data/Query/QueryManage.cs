using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace LM.Component.Data.Query
{
    /// <summary>
    ///  Describe:
    ///     This class is also the extension for SqlMapper.
    ///     Mainly used for Query for Multi Tables, Query Scalar and NoQuery.
    ///  Author: Catom
    /// </summary>
    public sealed class QueryManage
    {
        public DbSession DbSession { get; private set; }

        public QueryManage(DbSession dbSession)
        {
            DbSession = dbSession;
        }

        #region Get Query

        /// <summary>
        ///  Mysql or Sqlserver通用原生sql列表查询，用于分页。
        ///  推荐使用 GetListByPage 方法。
        /// </summary>
        /// <typeparam name="TModel">查询字段封装的模型</typeparam>
        /// <param name="querySql">列表查询语句</param>
        /// <param name="countSql">统计查询语句</param>
        /// <param name="count">统计的结果值</param>
        /// <param name="param">条件参数，支持字典、匿名对象</param>
        /// <returns></returns>
        private List<TModel> GetList<TModel>(string querySql, string countSql, out int count, object param = null)
        {
            var reader = DbSession.Connection.QueryMultiple(querySql + ";" + countSql);
            var list = reader.Read<TModel>().ToList();
            count = reader.Read<int>().FirstOrDefault();
            return list;
        }

        /// <summary>
        /// 原生 sql 列表查询。
        /// 使用：select * from [Novel] n 
        ///             inner join [Author] a on a.Id = n.AuthorId
        /// </summary>
        /// <typeparam name="TModel">查询的字段集合类</typeparam>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<TModel> GetList<TModel>(string query, object param = null)
        {
            return DbSession.Connection.Query<TModel>(query, param).ToList();
        }

        /// <summary>
        ///  多表同时查询。
        /// 使用：
        ///     conn.GetMultiList《User, Role》("select * from [User] where Id in @Id1; select * from [Role] where Id in @Id2", param);
        /// </summary>
        /// <typeparam name="TModel1"></typeparam>
        /// <typeparam name="TModel2"></typeparam>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<object> GetMultiList<TModel1, TModel2>(string query, object param = null)
        {
            var returns = new List<object>();
            var result = DbSession.Connection.QueryMultiple(query, param);
            var list1 = result.Read<TModel1>().ToList();
            var list2 = result.Read<TModel2>().ToList();
            returns.Add(list1);
            returns.Add(list2);
            return returns;
        }

        /// <summary>
        ///  sqlserver单表的分页查找，必须要将主表的 Id 字段传递过去。
        ///  使用：
        ///     QueryManage.GetListByPage《Model》("dbo.[User]", out itemCount, 1, 20);
        /// 或者
        ///     QueryManage.GetListByPage《Model》("(select u.id, * from [User] u inner join [Novel] n on n.uid = u.id)", out itemCount, 1, 20);
        /// </summary>
        /// <typeparam name="TModel">实体类型</typeparam>
        /// <param name="queryTables">表名</param>
        /// <param name="itemCount">数据总条数</param>
        /// <param name="pageIndex">查找页</param>
        /// <param name="pageSize">页面量</param>
        /// <returns></returns>
        public IEnumerable<TModel> GetListByPage<TModel>(string queryTables, out int itemCount, int pageIndex = 1, int pageSize = 10)
        {
            var queryS = string.Format(@"
select top {0} * from (
    select top {2} row_number() over(order by Temp1.Id) as RowNumber,* from {1} Temp1
)Temp2
where Temp2.RowNumber > {3};", pageSize, queryTables, pageIndex * pageSize, (pageIndex - 1) * pageSize);

            var countS = string.Format(@"select count( Temp1.Id ) from {0} Temp1", queryTables);

            var data = DbSession.Connection.QueryMultiple(queryS + ";" + countS);
            var list = data.Read<TModel>();
            itemCount = data.Read<int>().FirstOrDefault();

            return list;
        }

        /// <summary>
        ///  执行一条 sql 语句，返回受影响的条数。
        ///  使用：delete * from [User] where Id in(@ids)
        /// </summary>
        /// <param name="noSql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int GetExecuteNoQuery(string noSql, object param = null)
        {
            return DbSession.Connection.Execute(noSql, param);
        }

        /// <summary>
        ///  执行一条查询单个值的sql语句，并返回这个值。
        /// 使用：select count(id) from [Novel]
        /// </summary>
        /// <typeparam name="TR">返回类型</typeparam>
        /// <param name="querySql">查询语句</param>
        /// <param name="param">注入参数</param>
        /// <returns></returns>
        public TR GetScalar<TR>(string querySql, object param = null)
        {
            return DbSession.Connection.ExecuteScalar<TR>(querySql, param);
        }
        #endregion


    }
}
