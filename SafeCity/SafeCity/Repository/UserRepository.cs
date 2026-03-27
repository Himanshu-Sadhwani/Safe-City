using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Data;
using SafeCity.DTOs;
using SafeCity.Utility;

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
            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException(ErrorMessages.User.RequestNull);
                }

                // Map DTO to Domain Entity
                var userDetails = request.ToUserRegisterRequest();

                var existingUser = await _context.Users.FirstOrDefaultAsync(temp => temp.Email == request.Email);
                if (existingUser != null)
                {
                    throw new Exception(ErrorMessages.User.EmailExists);
                }

                await _context.Users.AddAsync(userDetails);
                await _context.SaveChangesAsync();

                // Map Domain Entity back to Response DTO
                var response = UserResigterResponseExtension.ToUserRegisterResponse(userDetails);
                return response;
            }
            catch (Exception)
            {
                throw new Exception(ErrorMessages.Database.SaveFailed);
            }
        }
    }
}