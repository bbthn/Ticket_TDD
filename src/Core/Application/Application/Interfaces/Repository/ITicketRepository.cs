

using Core.Application.Interfaces.Repository;
using Core.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Interfaces.Repository
{
    public interface ITicketRepository  : IGenericRepository<Ticket>
    {

    } 
}
