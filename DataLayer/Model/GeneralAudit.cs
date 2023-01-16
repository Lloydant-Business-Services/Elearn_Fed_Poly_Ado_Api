using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class GeneralAudit : BaseModel
    {
        public long UserId { get; set; }
        public virtual User User { get; set; }
        public string ActionPerformed { get; set; }
        public string Client { get; set; }
        public DateTime ActionTime { get; set; }
        public string ActionTable { get; set; }
        public long RecordId { get; set; }
        public string InitialValue { get; set; }
        public string CurrentValue { get; set; }
    }
}
