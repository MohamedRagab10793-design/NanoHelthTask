using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NanoHealth.Data.Entities;

namespace NanoHealth.Data.Configurations;

public class WorkflowStepConfiguration : IEntityTypeConfiguration<WorkflowStep>
{
    public void Configure(EntityTypeBuilder<WorkflowStep> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.StepName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(e => e.AssignedTo)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(e => e.ActionType)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(e => e.NextStep)
            .HasMaxLength(100);
        

        
        

        builder.HasOne(e => e.Workflow)
            .WithMany(w => w.Steps)
            .HasForeignKey(e => e.WorkflowId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
