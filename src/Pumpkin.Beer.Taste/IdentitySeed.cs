using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pumpkin.Beer.Taste.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pumpkin.Beer.Taste
{
    public class IdentitySeed
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolesManager;
        private readonly ILogger _logger;

        public IdentitySeed(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILoggerFactory loggerFactory)
        {
            _context = context;
            _userManager = userManager;
            _rolesManager = roleManager;
            _logger = loggerFactory.CreateLogger<IdentitySeed>();
        }

        public async Task CreateRoles()
        {
            if (await _context.Roles.AnyAsync())
            {
                _logger.LogInformation("Exists Roles.");
                return;
            }

            var adminRole = Constants.AdminRole;
            var adminName = Constants.InitialAdminName;
            var roleNames = new String[] { adminRole, Constants.ManagerRole };

            foreach (var roleName in roleNames)
            {
                var role = await _rolesManager.RoleExistsAsync(roleName);
                if (!role)
                {
                    var result = await _rolesManager.CreateAsync(new IdentityRole { Name = roleName });
                    _logger.LogInformation("Created roles {0}: {1}", roleName, result.Succeeded);
                }
            }

            var maybeAdmin = await _userManager.FindByNameAsync(adminName);
            if (maybeAdmin == null)
            {
                await _userManager.CreateAsync(new IdentityUser(adminName), Constants.InitialAdminPassword);
                _logger.LogInformation("Created default admin {0}", adminName);
            }

            var admin = await _userManager.FindByNameAsync(adminName);
            await _userManager.AddToRoleAsync(admin, adminRole);
            _logger.LogInformation("Added {0} to {1}", adminName, adminRole);
        }
    }
}
