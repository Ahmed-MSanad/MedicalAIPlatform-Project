using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalProj.Data.Contracts;
using MedicalProj.Data.Models;

namespace Services.Specifications
{
    public class AdminPhonesWithFilterSpecification : Specification<AdminPhone>
    {
        public AdminPhonesWithFilterSpecification(string adminId) : base(phone => phone.AdminId == adminId)
        {
        }
    }
}
