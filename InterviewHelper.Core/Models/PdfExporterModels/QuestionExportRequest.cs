﻿namespace InterviewHelper.Core.Models.PdfExporterModels;

public class QuestionExportRequest
{
    public DateTime InterviewDate { get; set; }
    public string IntervieweePosition { get; set; }
    public List<int> Questions { get; set; }
}