/**********************************
*
* 联盟数据库查询文件
*
***********************************/

use [Test] 
go

-- 取上一条写入的数据的Identity类型的字段值
select @@identity as LastID


select * from dbo.[User]  

insert into dbo.[User](Name,Age)
values('a6',6)
go
















