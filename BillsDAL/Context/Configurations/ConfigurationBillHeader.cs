using BillsEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsDAL.Context.Configurations
{
    public class ConfigurationBillHeader : IEntityTypeConfiguration<BillHeader>
    {
        public void Configure(EntityTypeBuilder<BillHeader> builder)
        {
            builder.HasOne(o => o.Store)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
