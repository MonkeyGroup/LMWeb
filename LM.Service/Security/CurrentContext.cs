using LM.Service.BootStrapIoC;
using LM.Utility.Util;
using Microsoft.Practices.Unity;

namespace LM.Service.Security
{
    public class CurrentContext
    {
        public static void SetUser(CurrentUser user)
        {
            var mySession = UnityBootStrapper.Instance.UnityContainer.Resolve<ISession>();
            mySession.Set("Current_User", user);
        }

        public static CurrentUser GetCurrentUser()
        {
            var mySession = UnityBootStrapper.Instance.UnityContainer.Resolve<ISession>();
            var user = mySession["Current_User"] as CurrentUser;
            // 游客状态下为CurrentUser=null
            if (user != null) return user;
            user = new CurrentUser();
            mySession.Set("Current_User", user);
            return user;
        }

        public static void ClearUser()
        {
            var mySession = UnityBootStrapper.Instance.UnityContainer.Resolve<ISession>();
            var user = mySession.Get<CurrentUser>("Current_User");
            if (user == null) return;
            var empty = new CurrentUser();
            mySession.Set("Current_User", empty);
        }


        #region 通用方法
        public static void Set(string key, object model, int defaultExpireMinutes = 30)
        {
            var mySession = UnityBootStrapper.Instance.UnityContainer.Resolve<ISession>();
            mySession.Set(key, model, defaultExpireMinutes);
        }

        public static object Get(string key)
        {
            var mySession = UnityBootStrapper.Instance.UnityContainer.Resolve<ISession>();
            return mySession[key];
        }

        #endregion

    }
}