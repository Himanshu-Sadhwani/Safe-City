using System;
using SafeCity.Domain.Enum;
using SafeCity.Domain.Entity;

namespace SafeCity.DTOs.CrisisDtos
{
    public class CrisisResponseDto
    {
        public int CrisisID { get; set; }

        public CrisisType Type { get; set; }

        public string Location { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public CrisisSeverity Severity { get; set; }

        public string Status { get; set; } = string.Empty;

        public static CrisisResponseDto FromEntity(Crisis crisis)
        {
            return new CrisisResponseDto
            {
                CrisisID = crisis.CrisisID,
                Type = crisis.Type,
                Location = crisis.Location,
                Date = crisis.Date,
                Severity = crisis.Severity,
                Status = crisis.Status.ToString()
            };
        }
    }
}

