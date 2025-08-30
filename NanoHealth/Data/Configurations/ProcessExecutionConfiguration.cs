using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NanoHealth.Data.Entities;

namespace NanoHealth.Data.Configurations;

public class ProcessExecutionConfiguration : IEntityTypeConfiguration<ProcessExecution>
{
    public void Configure(EntityTypeBuilder<ProcessExecution> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.StepName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(e => e.PerformedBy)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(e => e.Action)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(e => e.Comments)
            .HasMaxLength(500);
        
        builder.Property(e => e.ValidationError)
            .HasMaxLength(500);
        
        builder.Property(e => e.ValidationPassed)
            .HasDefaultValue(true);

  
        builder.HasOne(e => e.Process)
            .WithMany(p => p.Executions)
            .HasForeignKey(e => e.ProcessId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
