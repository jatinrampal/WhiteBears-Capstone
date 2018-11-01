using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhiteBears.Models {
    public class Company {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public DateTime CompanyFormed { get; set; }

        public Company(string name, DateTime companyFormed) {
            Name = name;
            CompanyFormed = companyFormed;
        }

        public Company(int companyId, string name, DateTime companyFormed) {
            CompanyId = companyId;
            Name = name;
            CompanyFormed = companyFormed;
        }
    }
}