using ActiveDirectoryTest;
using System.DirectoryServices.AccountManagement;

class Program
{
    static void Main()
    {
        ActiveDirectoryUserModel activeDirectoryUserModel = new()
        {
            Domain = "domain.com",
            Email = "email@email.com",
            UserName = "username",
            Password = "password",
            AdminUserName = "username",
            AdminPassword = "password"
        };

        bool result = Login(activeDirectoryUserModel);

        if (result)
        {
            Console.WriteLine("Sisteme hoşgeldiniz");
        }
        else
        {
            Console.WriteLine("Kullanıcı adı veya parola hatalı!");
        }
    }

    static bool Login(ActiveDirectoryUserModel activeDirectoryUser)
    {
        using (PrincipalContext context = new(ContextType.Domain, activeDirectoryUser.Domain))
        {
            return context.ValidateCredentials(activeDirectoryUser.UserName, activeDirectoryUser.Password);
        }
    }

    static bool CheckIfUserIsExistByUserName(ActiveDirectoryUserModel activeDirectoryUser)
    {
        using (PrincipalContext context = new PrincipalContext(ContextType.Domain, activeDirectoryUser.Domain, activeDirectoryUser.AdminUserName, activeDirectoryUser.AdminPassword))
        {
            UserPrincipal user = UserPrincipal.FindByIdentity(context, activeDirectoryUser.UserName);
            return user != null;
        }
    }

    static bool CheckIfUserIsExistByEmail(ActiveDirectoryUserModel activeDirectoryUser)
    {
        using (PrincipalContext context = new PrincipalContext(ContextType.Domain, activeDirectoryUser.Domain, activeDirectoryUser.AdminUserName, activeDirectoryUser.AdminPassword))
        {
            UserPrincipal userPrincipal = new UserPrincipal(context)
            {
                EmailAddress = activeDirectoryUser.Email
            };

            using (PrincipalSearcher searcher = new PrincipalSearcher(userPrincipal))
            {
                UserPrincipal result = (UserPrincipal)searcher.FindOne();
                return result != null;
            }
        }
    }
}
