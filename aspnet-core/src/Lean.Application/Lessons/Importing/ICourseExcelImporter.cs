using System.IO;

namespace Lean.Lessons.Importing
{
    public interface ICourseExcelImporter
    {
        Course Import(Stream fileStream, string fileName);
    }
}