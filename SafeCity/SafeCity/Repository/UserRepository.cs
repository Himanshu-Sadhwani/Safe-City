using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Data;
using SafeCity.Domain.Enum;
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

        /// <summary>
        /// Handles the database logic for updating a user's password based on the provided email address.
        /// </summary>
        /// <param name="request"> The forgot password request containing email and hashed password.</param>
        /// <returns> A response DTO confirming password update operation.</returns>
        /// <exception cref="ArgumentNullException"> Thrown when the request object is null.</exception>
        /// <exception cref="Exception"> Thrown when the user does not exist or when saving to the database fails.</exception>
        public async Task<ForgotPasswordResponseDto> ForgotPassword(
            ForgotPasswordRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException(
                        ErrorMessages.User.RequestNull);
                }

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email && u.Status == UserStatus.Active);

                if (user == null)
                {
                    throw new Exception(ErrorMessages.User.EmailExists);
                }

                // Update password using request DTO logic
                request.UpdateUserPassword(user);

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                // Map updated entity to response DTO
                return user.ToForgotPasswordResponse();
            }
            catch
            {
                throw new Exception(ErrorMessages.Database.ForgotPasswordFailed);
            }
        }
    }
}