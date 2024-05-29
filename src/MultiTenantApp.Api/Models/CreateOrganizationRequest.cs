namespace MultiTenantApp.API.Models
{
    public class CreateOrganizationRequest
    {
        public string OrganizationName { get; set; }
        public string Slug { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
