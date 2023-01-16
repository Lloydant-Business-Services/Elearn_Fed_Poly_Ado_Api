using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IAuditService
    {
        Task<bool> CreateAudit(long userId, string action, string client, string actionTable, long recordId, string initialValue, string newValue);
    }
}
