using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Web.Configuration;
using WebApplication3.Models;

[assembly: OwinStartupAttribute(typeof(WebApplication3.Startup))]
namespace WebApplication3
{
    public partial class Startup
    {
        public static string App_ID = WebConfigurationManager.AppSettings["App_ID"];
        public static string API_Key = WebConfigurationManager.AppSettings["Rest_API_KEY"];
        public static string User_Key = WebConfigurationManager.AppSettings["User_Key"];
        public static string URL = WebConfigurationManager.AppSettings["URL"];
        public static string UserName = WebConfigurationManager.AppSettings["UserName"];
        public static string UserName_DataEntry = WebConfigurationManager.AppSettings["Username_DataEntry"];
        public static string Password = WebConfigurationManager.AppSettings["Password"];

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
			createRolesandUsers();

		}

		private void createRolesandUsers()
		{
			ApplicationDbContext context = new ApplicationDbContext();

			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
			var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

			// In Startup iam creating first Admin Role and creating a default Admin User 
			if (!roleManager.RoleExists("Admin"))
			{

				// first we create Admin rool
				var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
				role.Name = "Admin";
				roleManager.Create(role);

				//Here we create a Admin super user who will maintain the website				

				var user = new ApplicationUser();
				user.UserName = UserName;
				user.Email = UserName;

				string userPWD = Password;

				var chkUser = UserManager.Create(user, userPWD);

				//Add default User to Role Admin
				if (chkUser.Succeeded)
				{
					var result1 = UserManager.AddToRole(user.Id, "Admin");

				}
            
            }
			if (!roleManager.RoleExists("DataEntry"))
			{
				var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
				role.Name = "DataEntry";
				roleManager.Create(role);
                var user_DataEntry = new ApplicationUser();
                user_DataEntry.UserName = UserName_DataEntry;
                user_DataEntry.Email = UserName_DataEntry;

                string userPWD_Data = Password;

                var chkDataEntryUser = UserManager.Create(user_DataEntry, userPWD_Data);

                //Add default User to Role Admin
                if (chkDataEntryUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user_DataEntry.Id, "DataEntry");

                }
            }
		}


			// creating Creating Employee role 
			//if (!roleManager.RoleExists("Employee"))
			//{
			//	var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
			//	role.Name = "Employee";
			//	roleManager.Create(role);

			//}
		}
	}

