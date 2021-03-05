using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Lessons.Dto.Import
{
    public class CourseImportDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ModuleImportDto> Modules { get; set; }
    }

    public class ModuleImportDto
    {
        public string Key { get; set; }
        public List<LessonImportDto> Lessons { get; set; }
        public List<TagImportDto> Tags { get; set; }
    }

    public class TagImportDto
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public int InitialRating { get; set; }
    }

    public class LessonImportDto
    {
        public string Name { get; set; }
        public bool IsInitial { get; set; }
        public string LessonText { get; set; }
        public string LessonVideoHtml { get; set; }
        public string ActivityText { get; set; }
        public string ActivityVideoHtml { get; set; }
        public List<ProblemImportDto> Problems { get; set; }
    }

    public class ProblemImportDto
    {
        public int Number { get; set; }
        public string TaskDescription { get; set; }
        public ProblemType Type { get; set; }
        public List<ProblemAnswerOptionImportDto> ProblemAnswerOptions { get; set; }
        public List<ProbleTagRefDto> TagRef { get; set; }
    }

    public class ProbleTagRefDto
    {
        public string TagKey { get; set; }
        public int Rating { get; set; }
    }

    public class ProblemAnswerOptionImportDto
    {
        public bool IsCorrect { get; set; }
        public string Text { get; set; }
    }
}
