using Auth.DAL.Data.Entities;
using Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace Auth {
    public static class IdentityConfigure {
         public static async Task ConfigureIdentityAsync(this WebApplication app) {
            using var serviceScope = app.Services.CreateScope();
            var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
            var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();
            var config = app.Configuration.GetSection("DefaultUsersConfig");

            // Try to create Administrator Role
            var adminRole = await roleManager.FindByNameAsync(ApplicationRoleNames.Administrator);
            if (adminRole == null) {
                var roleResult = await roleManager.CreateAsync(new Role {
                    Name = ApplicationRoleNames.Administrator,
                    Type = RoleType.Administrator
                });
                if (!roleResult.Succeeded) {
                    throw new InvalidOperationException($"Unable to create {ApplicationRoleNames.Administrator} role.");
                }

                adminRole = await roleManager.FindByNameAsync(ApplicationRoleNames.Administrator);
            }

            // Try to create Cook Role
            var cook = await roleManager.FindByNameAsync(ApplicationRoleNames.Cook);
            if (cook == null) {
                var roleResult = await roleManager.CreateAsync(new Role {
                    Name = ApplicationRoleNames.Cook,
                    Type = RoleType.Cook
                });
                if (!roleResult.Succeeded) {
                    throw new InvalidOperationException($"Unable to create {ApplicationRoleNames.Cook} role.");
                }
            }

            // Try to create Manager Role
            var Manager = await roleManager.FindByNameAsync(ApplicationRoleNames.Manager);
            if (Manager == null) {
                var roleResult = await roleManager.CreateAsync(new Role {
                    Name = ApplicationRoleNames.Manager,
                    Type = RoleType.Manager
                });
                if (!roleResult.Succeeded) {
                    throw new InvalidOperationException($"Unable to create {ApplicationRoleNames.Manager} role.");
                }
            }

            // Try to create Courier Role
            var Courier = await roleManager.FindByNameAsync(ApplicationRoleNames.Courier);
            if (Courier == null) {
                var roleResult = await roleManager.CreateAsync(new Role {
                    Name = ApplicationRoleNames.Courier,
                    Type = RoleType.Courier
                });
                if (!roleResult.Succeeded) {
                    throw new InvalidOperationException($"Unable to create {ApplicationRoleNames.Courier} role.");
                }
            }
            var Customer = await roleManager.FindByNameAsync(ApplicationRoleNames.Customer);
            if (Customer == null) {
                var roleResult = await roleManager.CreateAsync(new Role {
                    Name = ApplicationRoleNames.Customer,
                    Type = RoleType.Customer
                });
                if (!roleResult.Succeeded) {
                    throw new InvalidOperationException($"Unable to create {ApplicationRoleNames.Courier} role.");
                }
            }

            // Try to create Administrator user
            var adminUser = await userManager.FindByNameAsync(config["AdminUserName"]);
            if (adminUser == null) {
                var userResult = await userManager.CreateAsync(new User {
                    UserName = config["AdminUserName"],
                    Email = config["AdminUserName"],
                    FullName = "smdy",
                    BirthDate = new DateTime(1990, 1, 1),
                    Gender = Genders.Male,

                }, config["AdminPassword"]) ;
                if (!userResult.Succeeded) {
                    throw new InvalidOperationException($"Unable to create {config["AdminUserName"]} user");
                }

                adminUser = await userManager.FindByNameAsync(config["AdminUserName"]);
            }

            if (!await userManager.IsInRoleAsync(adminUser, adminRole.Name)) {
                await userManager.AddToRoleAsync(adminUser, adminRole.Name);
            }
        }
    }
}
