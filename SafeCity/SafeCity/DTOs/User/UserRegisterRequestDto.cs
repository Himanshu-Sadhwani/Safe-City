using SafeCity.Domain.Entity;
namespace SafeCity.DTOs
{
    public class UserRegisterRequestDto
    {

        public string Name { get; set; }
        public int RoleID { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public User ToUserRegisterRequest()
        {
            return new User()
            {

                Name = Name,
                RoleID = RoleID,
                Email = Email,
                Phone = Phone,
                PasswordHash = Password,
                PasswordSalt = ConfirmPassword
            };
        }
    }
}
