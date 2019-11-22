using System.Data.Entity;
using RoamingSupport.Models;

namespace RoamingSupport.DAL
{
    public class RoamingSupportContext : DbContext
    {
        public DbSet<Request> Requests { get; set; }
    }
}