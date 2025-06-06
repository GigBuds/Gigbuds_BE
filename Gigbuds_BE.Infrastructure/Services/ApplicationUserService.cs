using Gigbuds_BE.Application.Interfaces;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Specifications;
using Gigbuds_BE.Application.Specifications.EmployerProfiles;
using Gigbuds_BE.Domain.Entities.Accounts;
using Gigbuds_BE.Domain.Entities.Constants;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Domain.Entities.Memberships;
using Gigbuds_BE.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gigbuds_BE.Infrastructure.Services
{
    internal class ApplicationUserService<T> : IApplicationUserService<T> where T : ApplicationUser
    {
        // ================================
        // === Fields & Props
        // ================================

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        // ================================
        // === Constructors
        // ================================
        public ApplicationUserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        // ================================
        // === Methods
        // ================================

        public async Task<IReadOnlyList<T>> GetAllUsersAsync()
        {
            return await _userManager.Users.OfType<T>().ToListAsync();
        }

        public async Task<IList<ApplicationUser>> GetUsersByRoleAsync(string role)
        {
            return await _userManager.GetUsersInRoleAsync(role);
        }

        public async Task<IReadOnlyList<T>> GetAllUsersWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        public async Task<T?> GetUserWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            var query = _userManager.Users.OfType<T>().AsQueryable();
            return await SpecificationQueryBuilder<T>.BuildCountQuery(query, spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            var query = _userManager.Users.OfType<T>().AsQueryable();
            return SpecificationQueryBuilder<T>.BuildQuery(query, spec);
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            await _userManager.UpdateAsync(user);
        }

        public async Task InsertJobSeekerAsync(ApplicationUser user, string password)
        {
            var existingUserByPhoneNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == user.PhoneNumber);
            if (existingUserByPhoneNumber != null)
            {
                throw new DuplicateUserException($"A user with the phone number {user.PhoneNumber} already exists.");
            }

            var existingUserByEmail = await _userManager.FindByEmailAsync(user.Email!);
            if (existingUserByEmail != null)
            {
                throw new DuplicateUserException($"A user with the email {user.Email} already exists.");
            }


            var existingUserBySocialSecurityNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.SocialSecurityNumber == user.SocialSecurityNumber);
            if (existingUserBySocialSecurityNumber != null)
            {
                throw new DuplicateUserException($"A user with the social security number {user.SocialSecurityNumber} already exists.");
            }

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new CreateFailedException($"User creation failed: {errors}");
            }
        }

        public async Task AssignRoleAsync(T user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<string> GetUserRoleAsync(T user)
        {
            return (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? string.Empty;
        }

        public async Task<ApplicationUser?> GetByIdAsync(int userId, List<string>? includes = null, bool isTracking = false)
        {
            var query = _userManager.Users.AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include.ToString()));
            }

            return isTracking ? await query.FirstOrDefaultAsync(u => u.Id == userId) : await query.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task InsertEmployerAsync(ApplicationUser user, string password, string companyEmail)
        {
            // Check if CompanyEmail already exists (only if not empty)
            if (!string.IsNullOrEmpty(companyEmail))
            {
                var employerProfileSpec = new EmployerProfileSpecification(companyEmail);
                var existingEmployerByCompanyEmail = await _unitOfWork.Repository<EmployerProfile>().GetBySpecificationAsync(employerProfileSpec);
                if (existingEmployerByCompanyEmail != null)
                {
                    throw new DuplicateUserException($"A company with the email {companyEmail} already exists.");
                }
            }

            var existingUserByPhoneNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == user.PhoneNumber);
            if (existingUserByPhoneNumber != null)
            {
                if (await _userManager.IsInRoleAsync(existingUserByPhoneNumber, UserRoles.Employer))
                {
                    throw new DuplicateUserException($"A user with the phone number {user.PhoneNumber} already exists.");
                }
                else
                {
                    var checkEmailExist = await _userManager.FindByEmailAsync(user.Email);
                    if (checkEmailExist != null && existingUserByPhoneNumber.Email != user.Email)
                    {
                        throw new DuplicateUserException($"A user with the email {user.Email} already exists.");
                    }

                    var emptyEmployerProfileForExistingUser = new EmployerProfile
                    {
                        Id = existingUserByPhoneNumber.Id,
                        CompanyEmail = companyEmail,
                        CompanyAddress = string.Empty,
                        TaxNumber = string.Empty,
                        BusinessLicense = string.Empty,
                        IsUnlimitedPost = false,
                    };
                    _unitOfWork.Repository<EmployerProfile>().Insert(emptyEmployerProfileForExistingUser);
                    await _unitOfWork.CompleteAsync();
                    await _userManager.AddToRoleAsync(existingUserByPhoneNumber, UserRoles.Employer);
                    return;
                }
            }

            var checkEmail = await _userManager.FindByEmailAsync(user.Email);
            if (checkEmail != null)
            {
                throw new DuplicateUserException($"A user with the email {user.Email} already exists.");
            }

            var existingUserBySocialSecurityNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.SocialSecurityNumber == user.SocialSecurityNumber);
            if (existingUserBySocialSecurityNumber != null)
            {
                throw new DuplicateUserException($"A user with the social security number {user.SocialSecurityNumber} already exists.");
            }

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new CreateFailedException($"User creation failed: {errors}");
            }

            var emptyEmployerProfile = new EmployerProfile
            {
                Id = user.Id,
                CompanyEmail = companyEmail,
                CompanyAddress = string.Empty,
                TaxNumber = string.Empty,
                BusinessLicense = string.Empty,
                IsUnlimitedPost = false,
            };
            _unitOfWork.Repository<EmployerProfile>().Insert(emptyEmployerProfile);
            await _unitOfWork.CompleteAsync();
            await _userManager.AddToRolesAsync(user, new List<string> { UserRoles.Employer, UserRoles.JobSeeker });
        }

        public async Task InsertAsync(ApplicationUser user, string password)
        {
            var existingUserByPhoneNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == user.PhoneNumber);
            if (existingUserByPhoneNumber != null)
            {
                throw new DuplicateUserException($"A user with the phone number {user.PhoneNumber} already exists.");
            }

            var existingUserByEmail = await _userManager.FindByEmailAsync(user.Email!);
            if (existingUserByEmail != null)
            {
                throw new DuplicateUserException($"A user with the email {user.Email} already exists.");
            }

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new CreateFailedException($"User creation failed: {errors}");
            }
        }

        public async Task<string> GetJobSeekerMembershipLevelAsync(int userId)
        {
            var jobSeeker = await GetByIdAsync(userId, ["AccountMemberships", "AccountMemberships.Membership"])
                            ?? throw new NotFoundException("Job seeker not found");
            var membership = jobSeeker.AccountMemberships.FirstOrDefault(m => m.Membership.MembershipType == MembershipType.JobSeeker)
                                        ?.Membership.Title;

            return membership!;
        }

        public async Task<string> GetEmployerMembershipLevelAsync(int userId)
        {
            var employer = await GetByIdAsync(userId, ["AccountMemberships", "AccountMemberships.Membership"])
                            ?? throw new NotFoundException("Employer not found");
            var membership = employer.AccountMemberships.FirstOrDefault(m => m.Membership.MembershipType == MembershipType.Employer)
                                        ?.Membership.Title;

            return membership!;
        }
    }
}
