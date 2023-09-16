﻿using System.Diagnostics.CodeAnalysis;

namespace PontoLegal.Shared.Messages;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Error
{
    public static class Department
    {
        public const string INVALID_PAGINATION     = "Invalid skip and/or take value";
        public const string DEPARTMENT_NOT_FOUNDED = "Department not founded";
        public const string ERROR_ADDING           = "Error adding Department";
        public const string ERROR_REMOVING         = "Error removing Department";
        public const string ERROR_UPDATING         = "Error updating Department";
        public const string INVALID_ID             = "Invalid Id";
        public const string INVALID_NAME           = "Department name can't be null and must be between 3 and 30 chars";
        public const string NAME_ALREADY_EXISTS    = "Name already exists";
    }

    public static class JobPosition
    {
        public const string INVALID_PAGINATION  = "Invalid skip and/or take value";
        public const string ID_IS_REQUIRED      = "Id for Job Position is required";
        public const string NAME_IS_REQUIRED    = "Name of Job Position is required";
        public const string INVALID_NAME        = "Job Position name can't be null and must be between 3 and 30 chars";
        public const string NAME_ALREADY_EXISTS = "Already exists a Job Position at this Department";
        public const string ERROR_ADDING        = "Error adding Department";
    }
}
