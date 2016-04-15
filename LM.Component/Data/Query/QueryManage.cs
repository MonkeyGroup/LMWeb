using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace LM.Component.Data.Query
{
    public sealed class QueryManage
    {
        public DbSession DbSession { get; private set; }

        public QueryManage(DbSession dbSession)
        {
            DbSession = dbSession;
        }

        #region Get Query

        /// <summary>
        ///  原生sql列表查询，用于分页。
        ///  推荐使用 GetListByPage 方法。
        /// </summary>
        /// <typeparam name="T">查询字段封装的模型</typeparam>
        /// <param name="querySql">列表查询语句</param>
        /// <param name="countSql">统计查询语句</param>
        /// <param name="count">统计的结果值</param>
        /// <param name="param">条件参数，支持字典、匿名对象</param>
        /// <returns></returns>
        private List<T> GetList<T>(string querySql, string countSql, out int count, object param = null)
        {
            var reader = DbSession.Connection.QueryMultiple(querySql + ";" + countSql);
            var list = reader.Read<T>().ToList();
            count = reader.Read<int>().FirstOrDefault();
            return list;
        }

        /// <summary>
        ///  原生 sql 列表查询。
        /// 使用：select * from [Novel] n 
        ///             inner join [Author] a on a.Id = n.AuthorId
        /// </summary>
        /// <typeparam name="T">查询的字段集合类</typeparam>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<T> GetList<T>(string query, object param = null)
        {
            return DbSession.Connection.Query<T>(query, param).ToList();
        }

        /// <summary>
        ///  单表的分页查找，必须要将主表的 Id 字段传递过去。
        ///  使用：
        ///     QueryManage.GetListByPage《Model》("dbo.[User]", out itemCount, 1, 20);
        /// 或者
        ///     QueryManage.GetListByPage《Model》("(select u.id, * from [User] u inner join [Novel] n on n.uid = u.id)", out itemCount, 1, 20);
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="queryTables">表名</param>
        /// <param name="itemCount">数据总条数</param>
        /// <param name="pageIndex">查找页</param>
        /// <param name="pageSize">页面量</param>
        /// <returns></returns>
        public IEnumerable<T> GetListByPage<T>(string queryTables, out int itemCount, int pageIndex = 1, int pageSize = 10)
        {
            var queryS = string.Format(@"
select top {0} * from (
    select top {2} row_number() over(order by Temp1.Id) as RowNumber,* from {1} Temp1
)Temp2
where Temp2.RowNumber > {3};", pageSize, queryTables, pageIndex * pageSize, (pageIndex - 1) * pageSize);

            var countS = string.Format(@"select count( Temp1.Id ) from {0} Temp1", queryTables);

            var data = DbSession.Connection.QueryMultiple(queryS + ";" + countS);
            var list = data.Read<T>();
            itemCount = data.Read<int>().FirstOrDefault();

            return list;
        }

        /// <summary>
        ///  执行一条 sql 语句，返回受影响的条数。
        ///  使用：delete * from [User] where Id in(@ids)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int GetExecuteNoQuery(string sql, object param = null)
        {
            return DbSession.Connection.Execute(sql, param);
        }

        /// <summary>
        ///  执行一条查询单个值的sql语句，并返回这个值。
        /// 使用：select count(id) from [Novel]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="querySql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T GetScalar<T>(string querySql, object param = null)
        {
            return DbSession.Connection.ExecuteScalar<T>(querySql, param);
        }
        #endregion


    }
}
