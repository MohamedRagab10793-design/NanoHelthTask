using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NanoHealth.Data.Entities;
using NanoHealth.Enums;

namespace NanoHealth.Data.Configurations;

public class ProcessConfiguration : IEntityTypeConfiguration<Process>
{
    public void Configure(EntityTypeBuilder<Process> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Initiator)
            .IsRequired()
            .HasMaxLength(50);
        
       
        builder.Property(e => e.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(ProcessStatus.Pending);
        
        builder.Property(e => e.CurrentStep)
            .HasMaxLength(100);

       
        builder.HasOne(e => e.Workflow)
            .WithMany(w => w.Processes)
            .HasForeignKey(e => e.WorkflowId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(e => e.Executions)
            .WithOne(pe => pe.Process)
            .HasForeignKey(pe => pe.ProcessId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
