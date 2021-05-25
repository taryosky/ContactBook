using ContactBook.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System;

namespace ContactBook.Data
{
    public class ContactBookContext : IdentityDbContext<User>
    {
        public ContactBookContext(DbContextOptions<ContactBookContext> options) : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Photo> Photos { get; set; }
    }
}
