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















