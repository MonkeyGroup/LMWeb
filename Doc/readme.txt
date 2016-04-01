
----------------------项目框架结构-----------------

1. Commponent
	ORM 层，ORM框架的封装拓展

2. Entity
	实体层，与数据表对应

3. Repo
	DAL 层，仓储方法（调用Component框架）
	
4. Utility
	工具方法层

5. Web 
	前端框架
	
6. Web/Services
	BLL 层，业务逻辑接口服务（调用Repo层，Model 和 Entity 的交换处）
	注：服务层


