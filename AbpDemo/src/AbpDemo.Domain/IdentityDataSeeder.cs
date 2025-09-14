using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;

namespace AbpDemo
{
    public class IdentityDataSeeder : IDataSeedContributor, ITransientDependency
    {
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly IIdentityUserRepository _userRepository;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly IdentityUserManager _userManager;
        private readonly IdentityRoleManager _roleManager;
        private readonly ICurrentTenant _currentTenant;

        public IdentityDataSeeder(
            IIdentityRoleRepository roleRepository,
            IIdentityUserRepository userRepository,
            ILookupNormalizer lookupNormalizer,
            IdentityUserManager userManager,
            IdentityRoleManager roleManager,
            ICurrentTenant currentTenant)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _lookupNormalizer = lookupNormalizer;
            _userManager = userManager;
            _roleManager = roleManager;
            _currentTenant = currentTenant;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context?.TenantId))
            {
                await SeedRolesAsync();
                await SeedUsersAsync();
            }
        }

        private async Task SeedRolesAsync()
        {
            if (await _roleRepository.FindByNormalizedNameAsync(_lookupNormalizer.NormalizeName("Manager")) == null)
            {
                await _roleRepository.InsertAsync(
                    new IdentityRole(
                        Guid.NewGuid(),
                        "Manager",
                        _currentTenant.Id
                    )
                    {
                        IsStatic = true,
                        IsPublic = true
                    }
                );
            }
        }

        private async Task SeedUsersAsync()
        {
            const string managerUserName = "manager1";
            const string managerPassword = "1q2w3E*";

            var normalizedUserName = _lookupNormalizer.NormalizeName(managerUserName);

            var managerUser = await _userRepository.FindByNormalizedUserNameAsync(
                normalizedUserName
            );

            if (managerUser == null)
            {
                managerUser = new IdentityUser(
                    Guid.NewGuid(),
                    managerUserName,
                    $"{managerUserName}@abp.io",
                    _currentTenant.Id
                );

                await _userManager.CreateAsync(managerUser, managerPassword);
                await _userManager.AddToRoleAsync(managerUser, "Manager");
            }
        }
    }
}
