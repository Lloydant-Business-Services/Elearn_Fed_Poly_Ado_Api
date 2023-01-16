using DataLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IDeveloperPatchService
    {
        Task<ExcelSheetUploadAggregation> ProcessAPIData(IEnumerable<StudentUploadModel> studentList, long departmentId);
        Task<IEnumerable<StudentUploadModel>> MockAPIData();
    }
}
