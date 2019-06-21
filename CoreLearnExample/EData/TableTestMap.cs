using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLearnExample.EData
{
    public class TableTestMap: IEntityTypeConfiguration<TableTest>
    {
        public void Configure(EntityTypeBuilder<TableTest> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Introduce).HasMaxLength(50);
            builder.Property(c => c.Describe).HasMaxLength(50);
            builder.Property(c => c.AddTime).HasColumnType("datetime");
            builder.ToTable("TableTest");
        }
    }
}
