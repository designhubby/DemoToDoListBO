using DemoToListBE.Data.Authentication;
using DemoToListBE.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoToListBE.Data.EntityConfiguration
{
    public class ToDoListConfiguration : IEntityTypeConfiguration<ToDoList>
    {
        public void Configure(EntityTypeBuilder<ToDoList> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.ApplicationUser)
                .WithOne(y => y.ToDoList)
                .HasForeignKey<ToDoList>(z => z.ApplicationUserId)
                .IsRequired();
        }
    }
}
