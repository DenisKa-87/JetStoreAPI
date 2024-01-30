namespace JetStoreAPI.Helpers
{
    public class UserParams : Params
    {
        

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Paternal { get; set; }
        public string?  Email { get; set; }
        public DateOnly? Birthday { get; set; }

        public static UserParams Create(string? name = null, string? surname = null, string? paternal = null, 
            string? email = null, string? birthday = null, string? order = null)
        {
            return new UserParams
            {
                Name = name,
                Surname = surname,
                Paternal = paternal,
                Email = email,
                Birthday = birthday == null ? null : DateOnly.Parse(birthday),
                Order = order
            };
        }
    }
}
