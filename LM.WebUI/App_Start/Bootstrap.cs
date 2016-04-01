using System.Collections.Specialized;
using System.Configuration;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;

using LM.Utility.Util;
using LM.Component.Data;

namespace LM.WebUI
{
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

        private IUnityContainer _container;

        public IUnityContainer UnityContainer
        {
            get
            {
                return _container;
            }
        }

        public void Initialise()
        {
            BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(_container));
        }

        public void BuildUnityContainer()
        {
            _container = new UnityContainer();

            NameValueCollection nvc = ConfigurationManager.AppSettings;

            // 注入 Session
            string serverList = nvc["ServerList"];
            string[] serverIp = serverList.Split(',');
            IMySession mySession = new MySession(serverIp, int.Parse(nvc["SessionExpireHours"]), nvc["SessionCookieDomain"], nvc["SessionArea"]);
            _container.RegisterInstance(typeof(IMySession), mySession);
            // 注入 Cache
            _container.RegisterType(typeof(ICache), typeof(AppCache), "AppCache");

            #region 注入数据库连接

            string writeConn = ConfigurationManager.ConnectionStrings["LMWrite"].ConnectionString;
            string readConn = ConfigurationManager.ConnectionStrings["LMRead"].ConnectionString;
            _container.RegisterType(typeof(IUnitOfWork), typeof(DefaultUnitOfWork), "WriteUnitOfWork", new InjectionConstructor(writeConn));
            _container.RegisterType(typeof(IUnitOfWork), typeof(DefaultUnitOfWork), "ReadUnitOfWork", new InjectionConstructor(readConn));

            #endregion


        }
    }
}

