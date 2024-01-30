namespace JetStoreAPI.DTO
{
    public class RegisterDto : LoginDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Paternal { get; set; }
        public DateOnly? DateOfBirth { get; set; }
    }
}
