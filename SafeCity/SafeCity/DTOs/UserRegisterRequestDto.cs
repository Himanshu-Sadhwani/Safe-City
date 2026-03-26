using SafeCity.Domain.Entity;
namespace SafeCity.DTOs
{
    public class UserRegisterRequestDto
    {
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string Name { get; set; }
        public int RoleID { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public User ToUserRegisterRequest()
        {
            return new User()
            {
                PasswordHash = PasswordHash,
                PasswordSalt = PasswordSalt,
                Name = Name,
                RoleID = RoleID,
                Email = Email,
                Phone = Phone,
            };
        }
    }
}
