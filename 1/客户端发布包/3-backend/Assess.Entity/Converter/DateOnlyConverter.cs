using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Assess.Entity.Converter
{
    internal class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyConverter()
            : base(v => v.ToDateTime(new TimeOnly(0)),
                   v => new DateOnly(v.Year, v.Month, v.Day))
        {
        }
    }
}
