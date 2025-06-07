using MedicalProj.Data.Contracts;
using MedicalProj.Data.Models;

namespace Services.Specifications
{
    public class AdminWithFilterSpecification : Specification<Admin>
    {
        public AdminWithFilterSpecification(string adminId) : base(admin => admin.Id == adminId)
        {
            AddInclude(admin => admin.AdminPhones);
        }
    }
}
