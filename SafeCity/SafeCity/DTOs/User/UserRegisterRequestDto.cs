using SafeCity.Domain.Entity;
namespace SafeCity.DTOs
{
    public class UserRegisterRequestDto
    {

        public string Name { get; set; }
        public int RoleID { get; set; } = 1;
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }

        public User ToUserRegisterRequest()
        {
            return new User()
            {

                Name = Name,
                RoleID = RoleID,
                Email = Email,
                Phone = Phone,
                Password = Password
            };
        }
    }
}
