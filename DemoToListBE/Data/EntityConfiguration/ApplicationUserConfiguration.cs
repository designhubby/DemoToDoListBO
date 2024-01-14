using DemoToListBE.Data.Authentication;
using DemoToListBE.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoToListBE.Data.EntityConfiguration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasOne(x => x.ToDoList)
                .WithOne(y => y.ApplicationUser)
                .HasForeignKey<ApplicationUser>(z => z.ToDoListId)
                .IsRequired(false); // ToDoListId is not required
        }
    }
}
