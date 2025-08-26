using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.EF;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DXApplication4.Module.BusinessObjects;
using Microsoft.Extensions.DependencyInjection;

namespace DXApplication4.Module.DatabaseUpdate
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion)
        {
        }

        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();

#if !RELEASE

            var defaultRole = CreateDefaultRole();
            var adminRole = CreateAdminRole();
            var userRole = CreateUserRole();   

            ObjectSpace.CommitChanges();

            UserManager userManager = ObjectSpace.ServiceProvider.GetRequiredService<UserManager>();

            if (userManager.FindUserByName<ApplicationUser>(ObjectSpace, "User") == null)
            {
                string EmptyPassword = "";
                _ = userManager.CreateUser<ApplicationUser>(ObjectSpace, "User", EmptyPassword, (user) =>
                {
                    user.Roles.Add(defaultRole);
                });
            }

            if (userManager.FindUserByName<ApplicationUser>(ObjectSpace, "Admin") == null)
            {
                string EmptyPassword = "";
                _ = userManager.CreateUser<ApplicationUser>(ObjectSpace, "Admin", EmptyPassword, (user) =>
                {
                    user.Roles.Add(adminRole);
                });
            }

            if (userManager.FindUserByName<ApplicationUser>(ObjectSpace, "kullanici") == null)
            {
                string EmptyPassword = "";
                _ = userManager.CreateUser<ApplicationUser>(ObjectSpace, "kullanici", EmptyPassword, (user) =>
                {
                    user.Roles.Add(userRole);
                });
            }

            ObjectSpace.CommitChanges();
#endif
        }

        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
        }

        PermissionPolicyRole CreateAdminRole()
        {
            PermissionPolicyRole adminRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "Administrators");
            if (adminRole == null)
            {
                adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                adminRole.Name = "Administrators";
                adminRole.IsAdministrative = true;
            }
            return adminRole;
        }

        PermissionPolicyRole CreateDefaultRole()
        {
            PermissionPolicyRole defaultRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(role => role.Name == "Default");
            if (defaultRole == null)
            {
                defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                defaultRole.Name = "Default";

                defaultRole.AddObjectPermissionFromLambda<ApplicationUser>(
                    SecurityOperations.Read,
                    cm => cm.ID == (Guid)CurrentUserIdOperator.CurrentUserId(),
                    SecurityPermissionState.Allow);

                defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(
                    SecurityOperations.Write, "ChangePasswordOnFirstLogon",
                    cm => cm.ID == (Guid)CurrentUserIdOperator.CurrentUserId(),
                    SecurityPermissionState.Allow);

                defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(
                    SecurityOperations.Write, "StoredPassword",
                    cm => cm.ID == (Guid)CurrentUserIdOperator.CurrentUserId(),
                    SecurityPermissionState.Allow);

                defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                defaultRole.AddObjectPermission<ModelDifference>(SecurityOperations.ReadWriteAccess, "UserId = ToStr(CurrentUserId())", SecurityPermissionState.Allow);
                defaultRole.AddObjectPermission<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, "Owner.UserId = ToStr(CurrentUserId())", SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
            }
            return defaultRole;
        }

        PermissionPolicyRole CreateUserRole()
        {
            var role = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "Kullanıcı");
            if (role == null)
            {
                role = ObjectSpace.CreateObject<PermissionPolicyRole>();
                role.Name = "Kullanıcı";

                role.AddTypePermissionsRecursively<Book>(SecurityOperations.Read, SecurityPermissionState.Allow);

                role.AddTypePermissionsRecursively<Loan>(SecurityOperations.Read, SecurityPermissionState.Allow);
                role.AddTypePermissionsRecursively<Loan>(SecurityOperations.Create, SecurityPermissionState.Allow);
                role.AddTypePermissionsRecursively<Loan>(SecurityOperations.Write, SecurityPermissionState.Allow);
                role.AddTypePermissionsRecursively<Loan>(SecurityOperations.Delete, SecurityPermissionState.Deny);

                role.AddTypePermissionsRecursively<Member>(SecurityOperations.Read, SecurityPermissionState.Allow);

                role.AddNavigationPermission(@"Application/NavigationItems/Items/Library", SecurityPermissionState.Allow);
            }
            return role;
        }
    }
}
