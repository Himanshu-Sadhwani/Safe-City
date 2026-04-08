using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Data;
using SafeCity.Domain.Entity;
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
        /// Fetches a user by the given ID for update purposes, including the associated role.
        /// </summary>
        /// <param name="userId">The unique ID of the user.</param>
        /// <returns>The user data mapped to update response DTO, or null if not found.</returns>
        public async Task<UserUpdateByAdminResponseDto?> GetUserForUpdateAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            return user?.ToUserUpdateByAdminResponse();
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

        public async Task<UserUpdateByAdminResponseDto> UpdateUser(int id, UserUpdateByAdminRequestDto request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), ErrorMessages.UserUpdate.UpdateUserRequest);
            }

            var user = await GetUserByIdAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException(ErrorMessages.UserUpdate.UserNotFound);
            }
            // Update allowed fields only if provided
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                user.Name = request.Name;
            }
            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                if (!PhoneNumberHelper.IsValidPhoneNumber(request.Phone))
                {
                    throw new ArgumentNullException(nameof(request), ErrorMessages.UserUpdate.InvalidPhoneNo);
                }
                user.Phone = request.Phone;
            }
            if (request.RoleID != 0)
            {
                user.RoleID = request.RoleID;
            }
            if (request.Status != 0)
            {
                if (request.Status != UserStatus.Inactive)
                {
                    throw new ArgumentNullException(nameof(request), ErrorMessages.UserUpdate.InvalidStatus);
                }
                user.Status = request.Status;
            }

            await _context.SaveChangesAsync();

            // Map Domain Entity to Response DTO
            return user.ToUserUpdateByAdminResponse();

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

        public async Task<User?> GetUserByEmailAndStatusAsync(string email, UserStatus status)
        {
            return await _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u =>
                    u.Email == email &&
                    u.Status == status
                );
        }

        /// <summary>
        /// Saves an audit log entry for user actions such as login.
        /// </summary>
        /// <param name="userId">The ID of the user performing the action.</param>
        /// <param name="action">The description of the action performed.</param>
        /// <returns>A task representing the asynchronous logging operation.</returns>
        public async Task SaveAuditLogAsync(int userId, string action)
        {
            var audit = new AuditLog
            {
                UserID = userId,
                Action = action,
                Resource = "Auth/Login",
                Timestamp = DateTime.UtcNow
            };

            _context.AuditLogs.Add(audit);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Soft-deletes a user by setting their status to Inactive.
        /// </summary>
        /// <param name="userId">The unique ID of the user to delete.</param>
        /// <returns>A confirmation string upon successful deletion.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no active user with the given ID exists.</exception>
        /// <exception cref="Exception">Thrown when a database error occurs during save.</exception>
        public async Task<string> DeleteUser(int userId)
        {
            var user = await _context.Users
        .Include(u => u.UserRole)
        .FirstOrDefaultAsync(u => u.UserID == userId && u.Status == UserStatus.Active);

            if (user == null)
                throw new KeyNotFoundException(ErrorMessages.UserDelete.UserNotFound);

            // Prevent deletion if the user is an Admin
            if (user.UserRole != null && user.UserRole.RoleName == UserRoleOption.Admin)
                throw new InvalidOperationException(ErrorMessages.UserDelete.AdminCannotBeDeleted);

            user.Status = UserStatus.Inactive;

            try
            {
                await _context.SaveChangesAsync();
                return ErrorMessages.UserDelete.DeactivateSuccess;
            }
            catch (Exception)
            {
                throw new Exception(ErrorMessages.Database.UpdateFailed);
            }
        }

    }
}