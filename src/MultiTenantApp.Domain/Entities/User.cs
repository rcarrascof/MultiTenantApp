namespace MultiTenantApp.Domain.Entities
{


    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int OrganizationId { get; set; }

        public Organization Organization { get; set; }
    }

}
