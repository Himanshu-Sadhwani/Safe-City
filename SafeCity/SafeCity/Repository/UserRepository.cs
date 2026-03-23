using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Data;
using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;
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

        public async Task<UserResponseDto> CreateUser(UserRequestDto userRequestDto)
        {
            var user = userRequestDto.ToUserRequest();
            user.PasswordHash = new PasswordHasher<UserRequestDto>().HashPassword(userRequestDto, userRequestDto.PasswordHash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return UserResponseExtension.ToUserResponse(user);
        }

        public async Task<UserResponseDto> GetUserById(int id)
        {
            var user = await _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.UserID == id);

            return user == null ? null : UserResponseExtension.ToUserResponse(user);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsers()
        {
            var users = await _context.Users
                .Include(u => u.UserRole)
                .ToListAsync();

            return users.Select(u => UserResponseExtension.ToUserResponse(u));
        }

        public async Task<IEnumerable<UserResponseDto>> SearchUsers(string searchTerm)
        {
            var users = await _context.Users
                .Include(u => u.UserRole)
                .Where(u => u.Name.Contains(searchTerm) || u.Email.Contains(searchTerm))
                .ToListAsync();

            return users.Select(u => UserResponseExtension.ToUserResponse(u));
        }

        public async Task<UserResponseDto> UpdateUser(int id, UserRequestDto userRequestDto)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null) return null;

            existingUser.Name = userRequestDto.Name;
            existingUser.Email = userRequestDto.Email;
            existingUser.Phone = userRequestDto.Phone;
            existingUser.RoleID = userRequestDto.RoleID;
            existingUser.Status = userRequestDto.Status;
            await _context.SaveChangesAsync();
            return UserResponseExtension.ToUserResponse(existingUser);
        }

        public async Task<bool> UpdateUserStatus(int id, bool isActive)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.Status = isActive ? UserStatus.Active : UserStatus.Inactive;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.Status = UserStatus.Inactive;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<LoginResponseDto> LoginUser(LoginRequestDto loginRequestDto)
        {
            var user = await _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.Email == loginRequestDto.Email);

            if (user == null) return null;

            var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, loginRequestDto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            string token = GenerateJwtToken(user);
            return LoginResponseExtension.ToLoginResponse(user, token);
        }

        private string GenerateJwtToken(User user)
        {
            throw new NotImplementedException();
        }
    }
}