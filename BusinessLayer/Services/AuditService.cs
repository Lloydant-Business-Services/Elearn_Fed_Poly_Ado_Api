using BusinessLayer.Interface;
using DataLayer.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class AuditService : Repository<GeneralAudit>, IAuditService
    {
        public AuditService(ELearnContext context) : base(context)
        {

        }

        public async Task<bool> CreateAudit(long userId, string action, string client, string actionTable, long recordId, string initialValue, string newValue)
        {
            try
            {
                GeneralAudit audit = new GeneralAudit()
                {
                    UserId = userId,
                    ActionPerformed = action,
                    Client = client,
                    ActionTable = actionTable,
                    RecordId = recordId,
                    InitialValue = initialValue,
                    CurrentValue = newValue,
                    ActionTime = DateTime.Now,
                };
                _context.Add(audit);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
