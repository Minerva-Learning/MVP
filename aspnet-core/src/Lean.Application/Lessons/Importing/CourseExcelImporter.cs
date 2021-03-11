using Abp.Dependency;
using Abp.Localization;
using Abp.Localization.Sources;
using Lean.DataExporting.Excel.NPOI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lean.Lessons.Importing
{
    public class CourseExcelImporter : NpoiExcelImporterBase<Course>, ICourseExcelImporter, ITransientDependency
    {
        private readonly ILocalizationSource _localizationSource;

        public CourseExcelImporter(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(LeanConsts.LocalizationSourceName);
        }

        public Course Import(Stream fileStream, string fileName)
        {
            var workbook = new XSSFWorkbook(fileStream);

            var course = new Course { Name = fileName };
            var modulesMap = new Dictionary<int, Module>();
            Module GetOrCreateModule(int id)
            {
                if (modulesMap.ContainsKey(id))
                {
                    return modulesMap[id];
                }

                var module = new Module { Priority = id };
                modulesMap.Add(id, module);
                course.Modules.Add(module);
                return module;
            }

            var tagsSheet = workbook.GetSheet("Tags");
            var tagsMap = ProcessWorksheet(tagsSheet, new Dictionary<int, Tag>(), (sheet, ri, acc) =>
            {
                if (IsRowEmpty(sheet, ri))
                {
                    return;
                }

                var row = sheet.GetRow(ri);
                var tag = new Tag
                {
                    Name = GetStringValue(row.GetCell(2)),
                    InitialRating = (int)row.GetCell(3).NumericCellValue
                };
                var tagId = (int)row.GetCell(1).NumericCellValue;

                var moduleId = (int)row.GetCell(0).NumericCellValue;
                var module = GetOrCreateModule(moduleId);

                module.Tags.Add(tag);
                tag.ModuleFk = module;
                acc.Add(tagId, tag);
            });

            var lessonsSheet = workbook.GetSheet("Lessons");
            var lessonsMap = ProcessWorksheet(lessonsSheet, new Dictionary<int, Lesson>(), (sheet, ri, acc) =>
            {
                if (IsRowEmpty(sheet, ri))
                {
                    return;
                }

                var row = sheet.GetRow(ri);
                var lesson = new Lesson
                {
                    Name = GetStringValue(row.GetCell(2)),
                    IsInitial = (GetStringValue(row.GetCell(3))?.Trim()).EqualsIgnoreCase("y"),
                    LessonText = GetStringValue(row.GetCell(4)),
                    LessonVideoHtml = GetStringValue(row.GetCell(5)),
                    ActivityText = GetStringValue(row.GetCell(6)),
                    ActivityVideoHtml = GetStringValue(row.GetCell(7))
                };

                var moduleId = (int)row.GetCell(0).NumericCellValue;
                var module = GetOrCreateModule(moduleId);
                var lessonId = (int)row.GetCell(1).NumericCellValue;
                module.Lessons.Add(lesson);
                lesson.ModuleFk = module;
                acc.Add(lessonId, lesson);
            });

            var problemsSheet = workbook.GetSheet("Problems");
            var lessonProblemsMap = ProcessWorksheet(problemsSheet, new Dictionary<int, Dictionary<int, Problem>>(), (sheet, ri, acc) =>
            {
                if (IsRowEmpty(sheet, ri))
                {
                    return;
                }

                var row = sheet.GetRow(ri);
                var problemId = (int)row.GetCell(1).NumericCellValue;
                var problem = new Problem
                {
                    TaskDescription = GetStringValue(row.GetCell(2)),
                    Type = ProblemType.FreeText,
                    Number = problemId,
                    ProblemAnswerOptions = new List<ProblemAnswerOption>
                    {
                        new ProblemAnswerOption
                        {
                            IsCorrect = true,
                            Text = GetStringValue(row.GetCell(4))
                        }
                    }
                };
                var tagRefs = GetStringValue(row.GetCell(5))?.Split(",")?.Select(int.Parse)?.ToArray();
                var tagRatings = GetStringValue(row.GetCell(6))?.Split(",")?.Select(int.Parse)?.ToArray();
                if (tagRefs is not null && tagRatings is not null)
                {
                    for (int i = 0; i < tagRefs.Length; i++)
                    {
                        problem.ProblemTags.Add(new ProblemTag
                        {
                            TagFk = tagsMap[tagRefs[i]],
                            ProblemTagRating = tagRatings[i]
                        });
                    }
                }

                var lessonId = (int)row.GetCell(0).NumericCellValue;
                var lesson = lessonsMap[lessonId];
                lesson.Problems.Add(problem);
                problem.LessonFk = lesson;
                if (acc.ContainsKey(lessonId))
                {
                    acc[lessonId].Add(problemId, problem);
                }
                else
                {
                    acc.Add(lessonId, new Dictionary<int, Problem> { { problemId, problem } });
                }
            });


            var flowMapsSheet = workbook.GetSheet("FlowMaps");
            ProcessWorksheet(flowMapsSheet, new Dictionary<int, FlowRule>(), (sheet, ri, acc) =>
            {
                if (IsRowEmpty(sheet, ri))
                {
                    return;
                }

                var row = sheet.GetRow(ri);
                var lessonId = (int)row.GetCell(0).NumericCellValue;
                var problemIds = ParseProblemIds(GetStringValue(row.GetCell(1)));
                var lesson = lessonsMap[lessonId];
                var (condition, correctCount) = ParseFlowCondition(GetStringValue(row.GetCell(2)));
                var nextLessonIds = GetStringValue(row.GetCell(3)).Trim().Split(",")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(int.Parse);
                var flowRule = new FlowRule
                {
                    Priority = ri,
                    Condition = condition,
                    CorrectAnswersCount = correctCount,
                    FlowRuleProblems = problemIds.Select(pid => new FlowRuleProblem
                    {
                        ProblemFk = lessonProblemsMap[lessonId][pid]
                    }).ToList(),
                    FlowRuleNextLessons = nextLessonIds.Select((lid, i) => new FlowRuleNextLesson
                    {
                        NextLessonFk = lessonsMap[lid],
                        Priority = i
                    }).ToList()
                };
                lesson.FlowRules.Add(flowRule);
            });

            return course;
        }

        private (FlowCondition, int?) ParseFlowCondition(string condition)
        {
            condition = condition?.Trim();
            if (IsDefaultFlowRule(condition))
            {
                return (FlowCondition.Default, null);
            }

            var match = Regex.Match(condition, "(>|<)\\s*(\\d+)");
            var symbol = match.Groups[1].Value;
            var count = int.Parse(match.Groups[2].Value);
            return (symbol == ">" ? FlowCondition.MoreThan : FlowCondition.LessThan, count);
        }

        private List<int> ParseProblemIds(string range)
        {
            range = range?.Trim();
            if (IsDefaultFlowRule(range))
            {
                return new List<int>();
            }

            try
            {
                var rangeMatch = Regex.Match(range, "\\s*(\\d+)\\s*-\\s*(\\d+)\\s*");
                if (rangeMatch.Success && rangeMatch.Groups.Count > 2)
                {
                    var start = int.Parse(rangeMatch.Groups[1].Value);
                    var end = int.Parse(rangeMatch.Groups[2].Value);
                    return Enumerable.Range(start, end - start + 1).ToList();
                }
            }
            catch { }

            return range.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        }

        private bool IsDefaultFlowRule(string value) =>
            (string.IsNullOrEmpty(value) || value.EqualsIgnoreCase("*"));

        private T ProcessWorksheet<T>(ISheet worksheet, T accumulator, Action<ISheet, int, T> processExcelRow)
        {
            var rowEnumerator = worksheet.GetRowEnumerator();
            rowEnumerator.Reset();

            var i = 0;
            while (rowEnumerator.MoveNext())
            {
                if (i == 0)
                {
                    //Skip header
                    i++;
                    continue;
                }
                try
                {
                    processExcelRow(worksheet, i++, accumulator);
                }
                catch (Exception)
                {
                    throw;
                    //ignore
                }
            }

            return accumulator;
        }
    }
}
