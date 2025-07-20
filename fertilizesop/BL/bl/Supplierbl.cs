using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fertilizesop.BL.Models;
using fertilizesop.Interfaces.BLInterfaces;
using fertilizesop.Interfaces.DLinterfaces;

namespace fertilizesop.BL.Bl
{
    internal class Supplierbl : Isupplierbl
    {
        private readonly Isupplierdl _supplierdl;
        public List<Suppliers> s = new List<Suppliers>();

        public Supplierbl(Isupplierdl supplierdl)
        {
            _supplierdl = supplierdl;
        }
        public bool addsupplier(Suppliers s)
        {
            try
            {
                validatesuppliers(s);
                return _supplierdl.addsupplier(s);
            }
            catch(Exception e) 
            {
                throw new Exception("error in the adding supplier" + e.Message);
            }
        }

        public bool deletesupplier(Suppliers s)
        {
            throw new NotImplementedException();
        }

        public List<Suppliers> getsupplier()
        {
            try
            {
                return _supplierdl.getsupplier();
            }
            catch(Exception e)
            {
                throw new Exception("Error in BL " + e.Message);
            }
        }

        public List<Suppliers> searchsupplier(string text)
        {
            try
            {
                return _supplierdl.searchsupplier(text);
            }
            catch(Exception e)
            {
                throw new Exception("Error in searching in BL" +e.Message);
            }
            throw new NotImplementedException();
        }

        public bool updatesupplier(Suppliers s)
        {
            throw new NotImplementedException();
        }
        private void validatesuppliers(Suppliers s)
        {
            if(s.first_Name == null)
            {
                throw new Exception("Name cannot be empty");
            }
        }

    }
}
