/**********************************
*
* 联盟数据库查询文件
*
***********************************/

use [Test] 
go

--------------------------------- 分页相关 ---------------------------------

-- 1. 取上一条写入的数据的Identity类型的字段值
select @@identity as [LastID]


-- 2. 分页查询
--	pageIndex = 3,pageSize = 8
select top 8 * from (
	select top 32 row_number() over(order by id) as RowNumber,* from Novel
)A
where A.rownumber > 3*8

-- 3. 条件查询：CharIndex、PatIndex、like 
select a.SaveAt,a.*,PATINDEX('%啊%',a.title) from [Article] a
where CHARINDEX('啊',a.title)>0



--------------------------- 查看数据库连接数 ---------------------------------
SELECT * FROM
[Master].[dbo].[SYSPROCESSES] WHERE [DBID] IN ( SELECT 
   [DBID]
FROM 
   [Master].[dbo].[SYSDATABASES]
WHERE 
   NAME='LM'
)


---------------------------- 获取数据库信息 --------------------------
-- 所有数据库
SELECT Name FROM Master..SysDatabases 
ORDER BY Name

-- 所有自定义模式（用户）
SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA 
WHERE SCHEMA_OWNER = 'dbo'

-- 所有表
SELECT Name FROM SysObjects 
Where XType='U' 
ORDER BY Name

-- 查找模式下面的表
SELECT TABLE_SCHEMA,TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'dbo'
ORDER BY TABLE_SCHEMA, TABLE_NAME ASC

-- 表信息
SELECT * FROM SysObjects a 
WHERE a.name = 'User'

-- 表字段信息
SELECT * FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'User' 

-- * 查询哪些表有用到此字段
SELECT distinct TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS
WHERE COLUMN_NAME like '%Site%'

-- 查询所有表、字段的详细信息
SELECT 
    TableName=CASE WHEN C.column_id=1 THEN O.name ELSE N'' END,
    TableDesc=ISNULL(CASE WHEN C.column_id=1 THEN PTB.[value] END,N''),
    Column_id=C.column_id,
    ColumnName=C.name,
    PrimaryKey=ISNULL(IDX.PrimaryKey,N''),
    [IDENTITY]=CASE WHEN C.is_identity=1 THEN N'√'ELSE N'' END,
    Computed=CASE WHEN C.is_computed=1 THEN N'√'ELSE N'' END,
    Type=T.name,
    Length=C.max_length,
    Precision=C.precision,
    Scale=C.scale,
    NullAble=CASE WHEN C.is_nullable=1 THEN N'√'ELSE N'' END,
    [Default]=ISNULL(D.definition,N''),
    ColumnDesc=ISNULL(PFD.[value],N''),
    IndexName=ISNULL(IDX.IndexName,N''),
    IndexSort=ISNULL(IDX.Sort,N''),
    Create_Date=O.Create_Date,
    Modify_Date=O.Modify_date
FROM sys.columns C
    INNER JOIN sys.objects O
        ON C.[object_id]=O.[object_id]
            AND O.type='U'
            AND O.is_ms_shipped=0
    INNER JOIN sys.types T
        ON C.user_type_id=T.user_type_id
    LEFT JOIN sys.default_constraints D
        ON C.[object_id]=D.parent_object_id
            AND C.column_id=D.parent_column_id
            AND C.default_object_id=D.[object_id]
    LEFT JOIN sys.extended_properties PFD
        ON PFD.class=1 
            AND C.[object_id]=PFD.major_id 
            AND C.column_id=PFD.minor_id
--             AND PFD.name='Caption'  -- 字段说明对应的描述名称(一个字段可以添加多个不同name的描述)
    LEFT JOIN sys.extended_properties PTB
        ON PTB.class=1 
            AND PTB.minor_id=0 
            AND C.[object_id]=PTB.major_id
--             AND PFD.name='Caption'  -- 表说明对应的描述名称(一个表可以添加多个不同name的描述) 

    LEFT JOIN                       -- 索引及主键信息
    (
        SELECT 
            IDXC.[object_id],
            IDXC.column_id,
            Sort=CASE INDEXKEY_PROPERTY(IDXC.[object_id],IDXC.index_id,IDXC.index_column_id,'IsDescending')
                WHEN 1 THEN 'DESC' WHEN 0 THEN 'ASC' ELSE '' END,
            PrimaryKey=CASE WHEN IDX.is_primary_key=1 THEN N'√'ELSE N'' END,
            IndexName=IDX.Name
        FROM sys.indexes IDX
        INNER JOIN sys.index_columns IDXC
            ON IDX.[object_id]=IDXC.[object_id]
                AND IDX.index_id=IDXC.index_id
        LEFT JOIN sys.key_constraints KC
            ON IDX.[object_id]=KC.[parent_object_id]
                AND IDX.index_id=KC.unique_index_id
        INNER JOIN  -- 对于一个列包含多个索引的情况,只显示第1个索引信息
        (
            SELECT [object_id], Column_id, index_id=MIN(index_id)
            FROM sys.index_columns
            GROUP BY [object_id], Column_id
        ) IDXCUQ
            ON IDXC.[object_id]=IDXCUQ.[object_id]
                AND IDXC.Column_id=IDXCUQ.Column_id
                AND IDXC.index_id=IDXCUQ.index_id
    ) IDX
        ON C.[object_id]=IDX.[object_id]
            AND C.column_id=IDX.column_id 

-- WHERE O.name=N'要查询的表'       -- 如果只查询指定表,加上此条件
ORDER BY O.name,C.column_id



















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








