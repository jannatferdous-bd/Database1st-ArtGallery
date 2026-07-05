using ArtGallery.Models;
using System;
using System.Linq;
using System.Web.Security;

namespace ArtGallery
{
    public class MyRoleProvider : RoleProvider
    {
        public override string ApplicationName { get; set; } = "ArtGallery";

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            using (AppDbContext db = new AppDbContext())
            {
                foreach (string username in usernames)
                {
                    var user = db.tblUsers.FirstOrDefault(u => u.UserName == username);
                    if (user == null) continue;

                    foreach (string roleName in roleNames)
                    {
                        bool exists = db.tblRoles.Any(r => r.UserId == user.Id && r.RoleName == roleName);
                        if (!exists)
                        {
                            db.tblRoles.Add(new tblRole
                            {
                                UserId = user.Id,
                                RoleName = roleName
                            });
                        }
                    }
                }

                db.SaveChanges();
            }
        }

        public override void CreateRole(string roleName)
        {
            // Optional: role table আলাদা master হিসেবে maintain করছ না,
            // তাই এখানে কিছু করার দরকার নাই.
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            using (AppDbContext db = new AppDbContext())
            {
                var roles = db.tblRoles.Where(r => r.RoleName == roleName).ToList();

                if (throwOnPopulatedRole && roles.Any())
                {
                    throw new InvalidOperationException("Role is assigned to users.");
                }

                if (roles.Any())
                {
                    db.tblRoles.RemoveRange(roles);
                    db.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            using (AppDbContext db = new AppDbContext())
            {
                var users = (from r in db.tblRoles
                             join u in db.tblUsers on r.UserId equals u.Id
                             where r.RoleName == roleName && u.UserName.Contains(usernameToMatch)
                             select u.UserName)
                             .Distinct()
                             .ToArray();

                return users;
            }
        }

        public override string[] GetAllRoles()
        {
            using (AppDbContext db = new AppDbContext())
            {
                return db.tblRoles
                    .Select(r => r.RoleName)
                    .Distinct()
                    .ToArray();
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            using (AppDbContext db = new AppDbContext())
            {
                string[] roles = (from r in db.tblRoles
                                  join u in db.tblUsers on r.UserId equals u.Id
                                  where u.UserName == username
                                  select r.RoleName)
                                  .ToArray();

                return roles;
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            using (AppDbContext db = new AppDbContext())
            {
                return (from r in db.tblRoles
                        join u in db.tblUsers on r.UserId equals u.Id
                        where r.RoleName == roleName
                        select u.UserName)
                        .Distinct()
                        .ToArray();
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (AppDbContext db = new AppDbContext())
            {
                return (from r in db.tblRoles
                        join u in db.tblUsers on r.UserId equals u.Id
                        where u.UserName == username && r.RoleName == roleName
                        select r)
                        .Any();
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            using (AppDbContext db = new AppDbContext())
            {
                foreach (string username in usernames)
                {
                    var user = db.tblUsers.FirstOrDefault(u => u.UserName == username);
                    if (user == null) continue;

                    var rolesToRemove = db.tblRoles
                        .Where(r => r.UserId == user.Id && roleNames.Contains(r.RoleName))
                        .ToList();

                    if (rolesToRemove.Any())
                    {
                        db.tblRoles.RemoveRange(rolesToRemove);
                    }
                }

                db.SaveChanges();
            }
        }

        public override bool RoleExists(string roleName)
        {
            using (AppDbContext db = new AppDbContext())
            {
                return db.tblRoles.Any(r => r.RoleName == roleName);
            }
        }
    }
}