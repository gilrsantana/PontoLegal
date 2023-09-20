﻿using System.Diagnostics.CodeAnalysis;

namespace PontoLegal.Shared.Messages;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Error
{
    public static class Department
    {
        public const string DEPARTMENT_NOT_FOUNDED = "Department not founded";
        public const string ERROR_ADDING           = "Error adding Department";
        public const string ERROR_REMOVING         = "Error removing Department";
        public const string ERROR_UPDATING         = "Error updating Department";
        public const string INVALID_ID             = "Invalid Id";
        public const string INVALID_NAME           = "Department name can't be null and must be between 3 and 30 chars";
        public const string INVALID_PAGINATION     = "Invalid skip and/or take value";
        public const string NAME_ALREADY_EXISTS    = "Name already exists";
    }

    public static class JobPosition
    {
        public const string ERROR_ADDING        = "Error adding Department";
        public const string ERROR_UPDATING      = "Error updating Job Position";
        public const string ERROR_REMOVING      = "Error removing Job Position";
        public const string ID_IS_REQUIRED      = "Id for Job Position is required";
        public const string INVALID_NAME        = "Job Position name can't be null and must be between 3 and 30 chars";
        public const string INVALID_PAGINATION  = "Invalid skip and/or take value";
        public const string NAME_ALREADY_EXISTS = "Already exists a Job Position at this Department";
        public const string NAME_IS_REQUIRED    = "Name of Job Position is required";
        public const string NOT_FOUNDED         = "Job Position not founded";
    }

    public static class Company
    {
        public const string ALREADY_EXISTS     = "Company already exists";
        public const string ERROR_ADDING       = "Error adding Company";
        public const string ERROR_UPDATING     = "Error updating Company";
        public const string ID_IS_REQUIRED     = "Id for Company is required";
        public const string INVALID_NAME       = "Company name can't be null and must be between 3 and 30 chars";
        public const string INVALID_PAGINATION = "Invalid skip and/or take value";
        public const string NAME_IS_REQUIRED   = "Name of Company is required";
        public const string NOT_FOUNDED        = "Company not founded";
    }

    public static class Cnpj
    {
        public const string INVALID_CNPJ_DIGITS = "CNPJ invalid Check Digits";
        public const string INVALID_CNPJ_FORMAT = "CNPJ invalid Format";
    }

    public static class WorkingDay
    {
        public const string ERROR_REMOVING      = "Error removing Working Day";
        public const string NOT_FOUNDED         = "Working Day not founded";
        public const string INVALID_PAGINATION  = "Invalid skip and/or take value";
        public const string ID_IS_REQUIRED      = "Id for Working Day is required";
        public const string NAME_ALREADY_EXISTS = "Name already exists";
        public const string NAME_IS_REQUIRED    = "Name of Working Day is required";
        public const string ERROR_ADDING        = "Error adding Working Day";
        public const string INVALID_TYPE        = "Working Day Type must be equal to the difference between End Work and Start Work";
        public const string INVALID_NAME        = "Working Day name can't be null and must be between 3 and 30 chars";
        public const string INVALID_START_WORK  = "Start Work must be less than End Work and Start Break";
        public const string INVALID_START_BREAK = "Start Break must be less than End Break and greater than Start Work";
        public const string INVALID_END_BREAK   = "End Break must be greater than Start Break";
    }
}
