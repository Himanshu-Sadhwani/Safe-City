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
        /// <summary>
        /// Handles the database logic for registering a user, including email uniqueness checks and persistence.
        /// </summary>
        /// <param name="request">The registration request containing user details.</param>
        /// <returns>A response DTO containing the mapped details of the newly created user.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the request object is null.</exception>
        /// <exception cref="Exception">Thrown when a user with the provided email already exists.</exception>
        public async Task<UserRegisterResponseDto> RegisterUser(UserRegisterRequestDto request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // Map DTO to Domain Entity
            var userDetails = request.ToUserRegisterRequest();

            var existingUser = await _context.Users.FirstOrDefaultAsync(temp => temp.Email == request.Email);
            if (existingUser != null)
            {
                throw new Exception("Email Already Exist");
            }

            await _context.Users.AddAsync(userDetails);
            await _context.SaveChangesAsync();

            // Map Domain Entity back to Response DTO
            var response = UserResigterResponseExtension.ToUserRegisterResponse(userDetails);
            return response;
        }
        /// <summary>
        /// Updates user details by an administrator in the database.
        /// </summary>
        /// <param name="request"> The request DTO containing user ID and updated fields such as name,
        /// phone, role, and status. </param>
        /// <returns> A response DTO containing the updated user information. </returns>
        /// <exception cref="Exception"> Thrown when the user with the specified ID is not found. </exception>
        /// <exception cref="ArgumentNullException">Thrown when the request object is null.</exception>

        public async Task<UserUpdateByAdminResponseDto> UpdateUserByAdmin(UserUpdateByAdminRequestDto request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == request.UserID);
            if (user == null)
                throw new InvalidOperationException("User not found");

            // Update allowed fields
            user.Name = request.Name;
            user.Phone = request.Phone;
            user.RoleID = request.RoleID;
            user.Status = request.Status;

            await _context.SaveChangesAsync();

            // Map Domain Entity back to Response DTO
            return user.ToUserUpdateByAdminResponse();
        }
    }
}