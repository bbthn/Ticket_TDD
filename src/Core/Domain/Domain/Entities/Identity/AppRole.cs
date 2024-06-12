using Microsoft.AspNetCore.Identity;

namespace Core.Domain.Entities.Identity
{
    public class AppRole : IdentityRole<Guid>
    {
        public AppRole()
        {
            
        }
        public AppRole(string rolename):base( rolename)
        {}
    }
}
