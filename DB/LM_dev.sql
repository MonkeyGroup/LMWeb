/**********************************
*
* �������ݿ��ѯ�ļ�
*
***********************************/

use [Test] 
go

-- 1. ȡ��һ��д������ݵ�Identity���͵��ֶ�ֵ
select @@identity as [LastID]


-- 2. ��ҳ��ѯ
--	pageIndex = 3,pageSize = 8
select top 8 * from (
	select top 32 row_number() over(order by id) as RowNumber,* from Novel
)A
where A.rownumber > 3*8


---------------------------------------------------------- �ݸ�ֽ --------------
use LM
go

select a.CreateAt,a.*,PATINDEX('%��%',a.title) from [Article] a
where PATINDEX('%��%',a.title)>0





select top 1 * from (
    select top 1 row_number() over(order by Temp1.CreateAt desc) as RowNumber,* from (select a.* from [Article] a  where 1=1  and ( 1=2  or CHARINDEX('�Ƽ�',a.Title)>0 or CHARINDEX('�Ƽ�',a.Keys)>0 )) Temp1
)Temp2
where Temp2.RowNumber > 0;


select count( Temp1.Id ) from (select a.* from [Article] a  where 1=1 ) Temp1








