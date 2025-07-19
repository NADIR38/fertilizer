using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fertilizesop.BL.Models;
using fertilizesop.BL.Models.persons;
using fertilizesop.DL;
using fertilizesop.Interfaces.BLinterfaces;
using fertilizesop.Interfaces.DLInterfaces;

namespace fertilizesop.BL.bl
{
    public class Customerbl : Icustomerbl
    {
        private readonly Icustomerdl  _icustomerdl;

        public Customerbl(Icustomerdl customerDL)
        {
            this._icustomerdl = customerDL ?? throw new ArgumentNullException(nameof(customerDL), "Data access layer cannot be null.");
        }

        public bool addcustomer(Ipersons p)
        {
            var customer = p as Customer ?? throw new ArgumentException("Expected a Customer instance.", nameof(p));

            validatecustomer(p);
            try
            {   
              return  _icustomerdl.addcustomer(customer);
            }
            catch(Exception ex)
            {
            throw new Exception("Error while adding the customer " + ex.Message);
            }
        }

        public bool deletecustomer(Ipersons p)
        {
            throw new NotImplementedException();
        }

        public List<Ipersons> getcustomers()
        {
            throw new NotImplementedException();
        }

        public List<Ipersons> searchcustomer()
        {
            throw new NotImplementedException();
        }

        public bool updatecustomer(Ipersons p)
        {
            throw new NotImplementedException();
        }

        private void validatecustomer(Ipersons p)
        {
            if(p.id < 0)
            {
                throw new Exception("Id cannot be negative");
            }
            
        }
    }
}
