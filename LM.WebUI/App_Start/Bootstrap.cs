using System.Configuration;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;

using LM.Utility.Util;
using LM.Component.Data;

namespace LM.WebUI
{
    /// <summary>
    ///  启动设置容器
    /// </summary>
    public sealed class Bootstrapper
    {
        private static Bootstrapper _strapper;

        public static Bootstrapper Instance
        {
            get
            {
                if (_strapper != null) return _strapper;
                _strapper = new Bootstrapper();
                return _strapper;
            }
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


            #region 注入 Session、Cookie、Cache 等

            ISession mySession = new MySession();
            UnityContainer.RegisterInstance(typeof(ISession), mySession);
            ICookie myCookie = new MyCookie();
            UnityContainer.RegisterInstance(typeof(ICookie), myCookie);
            ICache myCache = new MyCache();
            UnityContainer.RegisterInstance(typeof(ICache), myCache);

            #endregion


            #region 注入数据库连接

            string writeConn = ConfigurationManager.ConnectionStrings["LMWrite"].ConnectionString;
            string readConn = ConfigurationManager.ConnectionStrings["LMRead"].ConnectionString;
            UnityContainer.RegisterType(typeof(IUnitOfWork), typeof(DefaultUnitOfWork), "WriteUnitOfWork", new InjectionConstructor(writeConn, SqlType.SqlServer));
            UnityContainer.RegisterType(typeof(IUnitOfWork), typeof(DefaultUnitOfWork), "ReadUnitOfWork", new InjectionConstructor(readConn, SqlType.SqlServer));

            #endregion


            #region 注入业务逻辑层实例


            #endregion


        }
    }
}

