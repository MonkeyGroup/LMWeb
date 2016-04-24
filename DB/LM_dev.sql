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


---------------------------------------------------------- 草稿纸 --------------
use LM
go

select a.CreateAt,a.*,PATINDEX('%啊%',a.title) from [Article] a
where PATINDEX('%啊%',a.title)>0





select top 1 * from (
    select top 1 row_number() over(order by Temp1.CreateAt desc) as RowNumber,* from (select a.* from [Article] a  where 1=1  and ( 1=2  or CHARINDEX('科技',a.Title)>0 or CHARINDEX('科技',a.Keys)>0 )) Temp1
)Temp2
where Temp2.RowNumber > 0;


select count( Temp1.Id ) from (select a.* from [Article] a  where 1=1 ) Temp1








