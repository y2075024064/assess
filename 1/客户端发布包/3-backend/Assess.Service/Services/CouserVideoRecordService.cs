using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assess.Entity;
using Microsoft.EntityFrameworkCore;

namespace Assess.Service.Services;
public interface ICouserVideoRecordService : IBusinessServiceBase<CouserVideoRecord>
{

}
public class CouserVideoRecordService : BusinessServiceBase<CouserVideoRecord>, ICouserVideoRecordService
{
    public CouserVideoRecordService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }
}
