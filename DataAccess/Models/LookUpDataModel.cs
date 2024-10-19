using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class LookUpDataModel<T>
    {
        public T Value { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }


    }
}
