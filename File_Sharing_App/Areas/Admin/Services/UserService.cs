using AutoMapper;
using AutoMapper.QueryableExtensions;
using File_Sharing_App.Areas.Admin.Models;
using File_Sharing_App.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace File_Sharing_App.Areas.Admin.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbcontext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(
            ApplicationDbcontext context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            this._context = context;
            this._mapper = mapper;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }
        public async Task<OperationResult> ToggeleBlockUserAsync(string UserId)
        {
            var existedUser = await _context.Users.FindAsync(UserId);
            if(UserId == null)
            {
                return OperationResult.NotFound();
            }
            existedUser.IsBlocked = !existedUser.IsBlocked;
            _context.Update(existedUser);
            await _context.SaveChangesAsync();
            return OperationResult.Succeded();
        }

        public IQueryable<AdminUserViewModel> GetAll()
        {
            var result = _context.Users
                .ProjectTo<AdminUserViewModel>(_mapper.ConfigurationProvider)
               .OrderByDescending(n => n.CreatedDate);
            return result;
        }

        public IQueryable<AdminUserViewModel> GetBlockedUsers()
        {
            var result = _context.Users.Where(u=>u.IsBlocked)
                .ProjectTo<AdminUserViewModel>(_mapper.ConfigurationProvider).
                OrderByDescending(n => n.CreatedDate);
            return result;
        }

        public async Task<int> GetUserRegisterationCountAsync()
        {
            var count =await _context.Users.CountAsync();     
            return  count;
        }

        public async Task<int> GetUserRegisterationCountAsync(int Month)
        {
            var Year = DateTime.Now.Year;
            var count = await _context.Users.CountAsync(u=>u.CreatedDate.Month== Month&& u.CreatedDate.Year == Year);
            return count;
        }

        public IQueryable<AdminUserViewModel> Search(string term)
        {

            var result = _context.Users.Where(
            u => u.Email==term ||
            u.FristName.Contains(term) ||
            u.LastName.Contains(term)||
            (u.FristName+ " "+ u.LastName).Contains(term))
             .OrderByDescending(n=>n.CreatedDate)
            .ProjectTo<AdminUserViewModel>(_mapper.ConfigurationProvider);
            return result;
        }

        public async Task IntializeAsync()
        {
            if(! await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
               await _roleManager.CreateAsync(new IdentityRole { Name = UserRoles.Admin });
            }
            var AdminUser = "yossef@gmail.com";
            if(await _userManager.FindByEmailAsync(AdminUser)==null)
            {
                var User=new ApplicationUser() { 
                    Email=AdminUser,
                    UserName=AdminUser,
                    PhoneNumber="01060409120",
                    EmailConfirmed=true,
                    PhoneNumberConfirmed=true,
                    FristName="Yosef",
                    LastName="Ebrahim"
                };
              await  _userManager.CreateAsync(User, "Yosef@12345");
              await _userManager.AddToRoleAsync(User, UserRoles.Admin);
            }
        }
    }
}
