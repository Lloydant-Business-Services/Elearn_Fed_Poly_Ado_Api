using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class ExcelSheetUploadAggregation
    {
        public long SuccessfullUpload { get; set; }
        public long FailedUpload { get; set; }
        public List<StudentUploadModel> FailedStudentUploads { get; set; }
    }

    public class ExcelSheetUploadAggregationInstructor
    {
        public long SuccessfullUpload { get; set; }
        public long FailedUpload { get; set; }
        public List<InstructorUploadModel> FailedInstructorUploads { get; set; }
    }

    public class CoursePloadSheetAggregation
    {
        public long SuccessfullUpload { get; set; }
        public long FailedUpload { get; set; }
        public List<CourseUploadModel> FailedCourseUploads { get; set; }
    }
}
