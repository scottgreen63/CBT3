﻿namespace CBT3_Domain.Entities;

public class AuditLogEntry : BaseEntity
{
    public AuditLogEntry(AuditLogEntryID id):base(id)
    {
    }
    public string UserID { get; set; }
    public string Workstation { get; set; }
    public string EventDateTime { get; set; }
    public string MessageType { get; set; }
    public string Severity { get; set; }
    public string Module { get; set; }
    public string Function { get; set; }
    public string Description { get; set; }

    
}
public class AuditLogEntryID : BaseId<string>
{
    public AuditLogEntryID(string id) : base(id) { }
}