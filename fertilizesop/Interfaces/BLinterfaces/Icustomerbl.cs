using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fertilizesop.BL.Models;

namespace fertilizesop.Interfaces.BLinterfaces
{
    public interface Icustomerbl
    {
        bool addcustomer(Ipersons p);
        bool updatecustomer(Ipersons p);
        bool deletecustomer(Ipersons p);
        List<Ipersons> getcustomers();
        List<Ipersons> searchcustomer();
    }
}
