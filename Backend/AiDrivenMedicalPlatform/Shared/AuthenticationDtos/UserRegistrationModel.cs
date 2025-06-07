
namespace Shared.AuthenticationDtos
{
    public abstract class UserRegistrationModel
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public ICollection<string> Phones { get; set; } = new List<string>();
    }
}
