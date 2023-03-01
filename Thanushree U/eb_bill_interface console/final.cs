using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace eb_bill_interface
{
    public class Final
    {  
        static void Main() 
        { 
        IElectricityBill bill = new Billing();
        bill.Bill_recipt();            
        }
    }
}

