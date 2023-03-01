using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace eb_bill_interface
{
    public interface IElectricityBill
    {
        void Bill_recipt();
    }
}
