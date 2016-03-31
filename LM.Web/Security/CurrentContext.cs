using LM.Utility.Util;

namespace LM.Web.Security
{
    public class CurrentContext
    {
        public static void SetUser(CurrentUser user)
        {
            var mySession = Bootstrapper.Instance.UnityContainer.Resolve(typeof(IMySession), "mySession", null) as IMySession;
            mySession.Set("Current_User", user);
        }

        public static CurrentUser GetCurrentUser()
        {
            //var mySession = Bootstrapper.Instance.UnityContainer.Resolve<IMySession>();
            var mySession = Bootstrapper.Instance.UnityContainer.Resolve(typeof(IMySession), "mySession", null) as IMySession;
            var user = mySession["Current_User"] as CurrentUser;
            // 游客状态下为CurrentUser=null
            if (user == null)
            {
                user = new CurrentUser();
                mySession.Set("Current_User", user);
            }
            return user;
        }

        public static void ClearUser()
        {
            var mySession = Bootstrapper.Instance.UnityContainer.Resolve(typeof(IMySession), "mySession", null) as IMySession;
            var user = mySession.Get<CurrentUser>("Current_User");
            if (user != null)
            {
                var empty = new CurrentUser();
                mySession.Set("Current_User", empty);
            }
        }
    }
}