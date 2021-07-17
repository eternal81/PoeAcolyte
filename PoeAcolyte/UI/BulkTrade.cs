using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PoeAcolyte.DataTypes;
using PoeAcolyte.Service;

namespace PoeAcolyte.UI
{
    public class BulkTrade : Trade, IPoeTradeControl
    {

        private readonly BulkTradeControl _bulkTradeControl;
        public UserControl GetUserControl => _bulkTradeControl;
        public BulkTrade(PoeLogEntry entry, IPoeCommands commands): base(entry,commands)
        {
           
                _bulkTradeControl = new BulkTradeControl();
                
        }
        
        public void AddLogEntry(PoeLogEntry entry)
        {
            throw new NotImplementedException();
        }
        
    }
}