using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payments.Domain.Entities;

namespace Payments.Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("payments");

            builder.HasKey(p => p.PaymentId);

            builder.Property(p => p.PaymentId)
                .HasColumnName("payment_id");

            builder.Property(p => p.CustomerId)
                .HasColumnName("customer_id");

            builder.Property(p => p.ServiceProvider)
                .HasColumnName("service_provider")
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Amount)
                .HasColumnName("amount")
                .HasColumnType("decimal(12,2)")
                .IsRequired();

            builder.Property(p => p.Currency)
                .HasColumnName("currency")
                .HasConversion<string>()  // guarda enum como texto (BOB, USD)
                .HasMaxLength(3);

            builder.Property(p => p.Status)
                .HasColumnName("status")
                .HasConversion<string>()  // guarda enum como texto
                .HasMaxLength(20);

            builder.Property(p => p.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
