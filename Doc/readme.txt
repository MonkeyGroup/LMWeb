
----------------------项目框架结构-----------------

1. Commponent
	ORM 层，ORM框架的封装拓展

2. Model
	实体层 & 视图模型层 & 通用枚举参数

3. Repo
	DAL 层，仓储方法（调用Component框架）

4. Service
	BLL 层，业务逻辑接口服务（调用Repo层、Component层）
	校验功能 & 过滤功能 & 底层注入容器

5. Utility
	工具方法层

6. WebUI 
	前端框架
	


------------ 说明 ---------------

1. Model 和 Entity 的数据转换在Controller层进行；


