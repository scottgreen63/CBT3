﻿

namespace CBT3_Domain.Interfaces;

public interface IAuditableEntity : IBaseEntity
{
    string? CreatedBy { get; set; }
    DateTime? CreatedDate { get; set; }
    string? UpdatedBy { get; set; }
    DateTime? UpdatedDate { get; set; }
}
