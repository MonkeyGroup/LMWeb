using System.Configuration;
using System.Web.Mvc;
using LM.Component.Data;
using LM.Utility.Util;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;

namespace LM.Service.BootStrapIoC
{
    /// <summary>
    ///  启动设置容器
    /// </summary>
    public sealed class UnityBootStrapper
    {
        private static UnityBootStrapper _strapper;

        public static UnityBootStrapper Instance
        {
            get { return _strapper ?? (_strapper = new UnityBootStrapper()); }
        }

        public IUnityContainer UnityContainer { get; private set; }

        public void Initialise()
        {
            BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(UnityContainer));
        }

        public void BuildUnityContainer()
        {
            UnityContainer = new UnityContainer();


            #region 注入 Session、Cache。Session 是单例模式，应该注入实例；Cache 不是单例模式，需要注入类型。

            ISession mySession = new MySession();
            UnityContainer.RegisterInstance(typeof(ISession), mySession);
            UnityContainer.RegisterType<ICache, MyCache>();

            #endregion


            #region 注入数据库连接

            var writeConn = ConfigurationManager.ConnectionStrings["LMWrite"].ConnectionString;
            var readConn = ConfigurationManager.ConnectionStrings["LMRead"].ConnectionString;
            UnityContainer.RegisterType<DbSession>("WriteDbSession", new InjectionConstructor(writeConn));
            UnityContainer.RegisterType<DbSession>("ReadDbSession", new InjectionConstructor(readConn));

            #endregion


            #region 注入业务逻辑层实例

            UnityContainer.RegisterType<UserService>(new InjectionConstructor(UnityContainer.Resolve<DbSession>("WriteDbSession")));
            UnityContainer.RegisterType<HomePageConfigService>(new InjectionConstructor(UnityContainer.Resolve<DbSession>("WriteDbSession")));
            UnityContainer.RegisterType<ArticleService>(new InjectionConstructor(UnityContainer.Resolve<DbSession>("WriteDbSession")));
            UnityContainer.RegisterType<BaseInfoService>(new InjectionConstructor(UnityContainer.Resolve<DbSession>("WriteDbSession")));
            UnityContainer.RegisterType<CompanyService>(new InjectionConstructor(UnityContainer.Resolve<DbSession>("WriteDbSession")));
            UnityContainer.RegisterType<OperationLogService>(new InjectionConstructor(UnityContainer.Resolve<DbSession>("WriteDbSession")));
            UnityContainer.RegisterType<ProductService>(new InjectionConstructor(UnityContainer.Resolve<DbSession>("WriteDbSession")));
            UnityContainer.RegisterType<ExpertService>(new InjectionConstructor(UnityContainer.Resolve<DbSession>("WriteDbSession")));
            UnityContainer.RegisterType<BriefService>(new InjectionConstructor(UnityContainer.Resolve<DbSession>("WriteDbSession")));
            UnityContainer.RegisterType<CategoryService>(new InjectionConstructor(UnityContainer.Resolve<DbSession>("WriteDbSession")));
            UnityContainer.RegisterType<DbProcessService>(new InjectionConstructor(UnityContainer.Resolve<DbSession>("WriteDbSession")));
            UnityContainer.RegisterType<FruitsService>(new InjectionConstructor(UnityContainer.Resolve<DbSession>("WriteDbSession")));
            UnityContainer.RegisterType<AdService>(new InjectionConstructor(UnityContainer.Resolve<DbSession>("WriteDbSession")));

            #endregion

        }
    }
}

