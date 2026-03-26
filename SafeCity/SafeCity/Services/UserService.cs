    using SafeCity.DTOs;
    using SafeCity.Repository;
using SafeCity.Utility;
namespace SafeCity.Services;

    /// <summary>
    /// This service handles the logic for user registration, like validation and security.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        /// <summary>
        /// Validates and updates user details by an administrator.
        /// </summary>
        /// <param name="request"> The request DTO containing user ID and updated fields such as
        /// name, phone number, role, and status. </param>
        /// <returns> A response DTO containing the updated user information. </returns>
        /// <exception cref="ArgumentNullException"> Thrown when the request object is null. </exception>
        /// <exception cref="ArgumentException"> Thrown when provided data is invalid (e.g., invalid IDs or missing fields). </exception>

        public async Task<UserUpdateByAdminResponseDto> UpdateUserByAdmin(UserUpdateByAdminRequestDto request)
        {
            // Check if the request exists
            if (request == null)
                throw new ArgumentNullException(nameof(request),ErrorMessageUpdate.UserUpdate.RequestNull);

            // Validate that the UserID is a positive number
            if (request.UserID <= 0)
                throw new ArgumentNullException(nameof(request),ErrorMessageUpdate.UserUpdate.InvalidUserId);

            // Validate that the user's name is provided
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentNullException(nameof(request),ErrorMessageUpdate.UserUpdate.NameRequired);

            // Validate that the phone number is provided
            if (string.IsNullOrWhiteSpace(request.Phone))
                throw new ArgumentNullException(nameof(request),ErrorMessageUpdate.UserUpdate.PhoneRequired);

            // Validate that the RoleID is valid
            if (request.RoleID <= 0)
                throw new ArgumentNullException(nameof(request),ErrorMessageUpdate.UserUpdate.InvalidRoleId);;

            // Delegate persistence and data update logic to the repository layer
            return await _userRepository.UpdateUserByAdmin(request);

        }
    }