using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;

namespace SafeCity.DTOs
{
    public class UserRequestDto
    {
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string Name { get; set; }
        public int RoleID { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public UserStatus Status { get; set; }
        public virtual UserRole UserRole { get; set; }


        public User ToUserRequest()
        {
            return new User()
            {
                PasswordHash = PasswordHash,
                PasswordSalt = PasswordSalt,
                Name = Name,
                RoleID = RoleID,
                Email = Email,
                Phone = Phone,
                Status = Status,
                UserRole = UserRole
            };
        }
    }
}
