using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Data;
using SafeCity.DTOs;
namespace SafeCity.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SafeCityDbContext _context;
        public UserRepository(SafeCityDbContext context)
        {
            _context = context;
        }
        public async Task<UserRegisterResponseDto> RegisterUser(UserRegisterRequestDto request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var userDetails = request.ToUserRegisterRequest();

            var existingUser = await _context.Users.FirstOrDefaultAsync(temp => temp.Email == request.Email);
            if (existingUser != null)
            {
                throw new Exception("Email Already Exist");
            }
            await _context.Users.AddAsync(userDetails);
            await _context.SaveChangesAsync();
            var response = UserResigterResponseExtension.ToUserRegisterResponse(userDetails);
            return response;
        }

        public async Task<UserUpdateByAdminResponseDto> UpdateUserByAdmin(UserUpdateByAdminRequestDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == request.UserID);
            if (user == null)
                throw new Exception("User not found");
            user.Name = request.Name;
            user.Phone = request.Phone;
            user.RoleID = request.RoleID;
            user.Status = request.Status;
            await _context.SaveChangesAsync();
            return user.ToUserUpdateByAdminResponse();
        }
    }
}