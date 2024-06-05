using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistance.Context
{
    public class DesignTimeDbcontextFactory : IDesignTimeDbContextFactory<TicketDbContext>
    {
        private const string connectionString = "Data Source=172.167.76.241,1433;User Id=sa;Password=mypassword55!!;Initial Catalog=PostTicketing;Connect Timeout=60;Encrypt=False;Trust Server Certificate=True";
        //private const string connectionString = "Data Source=DESKTOP-QSHBLNM\\MSSQLSERVER2022;Initial Catalog=Ticket;Integrated Security=True;Connect Timeout=30;Trust Server Certificate=True;";
        public TicketDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(connectionString);
            return new TicketDbContext(builder.Options);
        }
    }
}
