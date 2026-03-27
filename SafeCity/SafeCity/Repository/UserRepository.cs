using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Data;
using SafeCity.Domain.Entity;
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
            if (request == null)
            {
                throw new ArgumentNullException(ErrorMessages.User.RequestNull);
            }

            // Check if email already exists BEFORE the try block so the catch doesn't overwrite it
            var existingUser = await _context.Users.FirstOrDefaultAsync(temp => temp.Email == request.Email);
            if (existingUser != null)
            {
                throw new Exception(ErrorMessages.User.EmailExists);
            }

            try
            {
                // Map DTO to Domain Entity
                var userDetails = request.ToUserRegisterRequest();

                await _context.Users.AddAsync(userDetails);
                await _context.SaveChangesAsync();

                // Map Domain Entity back to Response DTO
                var response = UserResigterResponseExtension.ToUserRegisterResponse(userDetails);
                return response;
            }
            catch (Exception)
            {
                // This will now only catch real database failures (like connection or length issues)
                throw new Exception(ErrorMessages.Database.SaveFailed);
            }
        }

        /// <summary>
        /// Fetches a user by the given ID, including the associated role.
        /// </summary>
        /// <param name="userId">The unique ID of the user.</param>
        /// <returns>The matching <see cref="User"/> entity, or null if not found.</returns>
        /// <exception cref="Exception">Thrown when a database error occurs.</exception>
        
        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.UserRole)
                    .FirstOrDefaultAsync(u => u.UserID == userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching user by Id", ex);
            }
        }

        /// <summary>
        /// Retrieves all users from the database along with their roles.
        /// </summary>
        /// <returns>A list of all <see cref="User"/> entities.</returns>
        /// <exception cref="Exception">Thrown when a database retrieval error occurs.</exception>
        
        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users
                    .Include(u => u.UserRole)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching all users", ex);
            }
        }
    /// <summary>
        /// Updates user details by an administrator in the database.
        /// </summary>
        /// <param name="request"> The request DTO containing user ID and updated fields such as name,
        /// phone, role, and status. </param>
        /// <returns> A response DTO containing the updated user information. </returns>
        /// <exception cref="Exception"> Thrown when the user with the specified ID is not found. </exception>
        /// <exception cref="ArgumentNullException">Thrown when the request object is null.</exception>

        public async Task<UserUpdateByAdminResponseDto> UpdateUser(UserUpdateByAdminRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request),ErrorMessages.UserUpdate.UpdateUserRequest);
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == request.UserID);

                if (user == null)
                {
                    throw new InvalidOperationException(ErrorMessages.UserUpdate.UserNotFound);
                }

                // Update allowed fields
                user.Name = request.Name;
                user.Phone = request.Phone;
                user.RoleID = request.RoleID;
                user.Status = request.Status;

                await _context.SaveChangesAsync();

                // Map Domain Entity to Response DTO
                return user.ToUserUpdateByAdminResponse();
            }
           catch (ArgumentNullException ex)
            {
                throw new ApplicationException(ErrorMessages.UserUpdate.UpdateUserRequest,ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new ApplicationException(ErrorMessages.UserUpdate.UserNotFound,ex);
            }
            catch (DbUpdateException ex)
            {
            throw new DbUpdateException(ErrorMessages.Database.UpdateFailed,ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ErrorMessages.User.InternalError,ex);
            }


        }

        public Task<ForgotPasswordResponseDto> ForgotPassword(ForgotPasswordRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}