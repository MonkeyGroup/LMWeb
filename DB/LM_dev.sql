/**********************************
*
* 联盟数据库查询文件
*
***********************************/

use [Test] 
go

-- 1. 取上一条写入的数据的Identity类型的字段值
select @@identity as [LastID]


-- 2. 分页查询
--	pageIndex = 3,pageSize = 8
select top 8 * from (
	select top 32 row_number() over(order by id) as RowNumber,* from Novel
)A
where A.rownumber > 3*8















