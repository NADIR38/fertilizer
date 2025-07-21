using fertilizesop.BL.Models;
using fertilizesop.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Bl
{
    public class SbilldetailsBl : ISbilldetailsBl
    {
        private readonly ISbilldetailsDl ibl;
        public SbilldetailsBl(ISbilldetailsDl ibl)
        {
            this.ibl = ibl;
        }
        public bool addrecord(Spricerecord s)
        {
            if (s.payement <= 0)
            {
                throw new ArgumentException("Payement should be greater than zero");
            }
            return ibl.addrecord(s);
        }

        public List<Supplierpayment> getdetails(int billid)
        {
            return ibl.getdetails(billid);
        }
    }
}
