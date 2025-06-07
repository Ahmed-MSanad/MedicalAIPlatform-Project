using AutoMapper;
using MedicalProj.Data.Contracts;
using MedicalProj.Data.Models;
using Microsoft.AspNetCore.Identity;
using Services.Abstraction;
using Services.Specifications;
using shared.AdminDtos;
using Shared.DoctorDtos;
using Shared.PatientDtos;

namespace Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public ProfileService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<PatientDto> GetPatientProfileService(string patientId)
        {
            var specification = new PatientWithFilterSpecification(patientId);

            var userDetails = await _unitOfWork.GetRepository<Patient, string>().GetByIdAsync(specification);

            if (userDetails == null)
            {
                throw new Exception("Patient not found in Get Patient Profile Service");
            }

            var patientDto = _mapper.Map<PatientDto>(userDetails);

            return patientDto;
        }

        public async Task<DoctorDto> GetDoctorProfileService(string doctorId)
        {
            var userDetails = await _unitOfWork.GetRepository<Doctor, string>().GetByIdAsync(doctorId);
            if(userDetails == null)
            {
                throw new Exception("doctor not found in Get Doctor Profile Service");
            }

            var doctorDto = _mapper.Map<DoctorDto>(userDetails);

            return doctorDto;
        }

        public async Task<AdminDto> GetAdminProfileService(string adminId)
        {
            var specification = new AdminWithFilterSpecification(adminId);

            var userDetails = await _unitOfWork.GetRepository<Admin, string>().GetByIdAsync(specification);
            if (userDetails == null)
            {
                throw new Exception("Admin not found in Get Admin Profile");
            }

            var adminDto = _mapper.Map<AdminDto>(userDetails);

            return adminDto;
        }

        public async Task DeleteUserProfileService(string userId)
        {
            var userDetails = await _userManager.FindByIdAsync(userId);

            if (userDetails == null) throw new Exception("User not found in Delete User Profile Service");

            var result = await _userManager.DeleteAsync(userDetails);
            if (!result.Succeeded)
            {
                throw new Exception($"{result.Errors}");
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task EditPatientProfileService(PatientDto updateRequest, string patientId)
        {
            var specification = new PatientWithFilterSpecification(patientId);

            var patient = await _unitOfWork.GetRepository<Patient, string>().GetByIdAsync(specification);
            if (patient == null)
            {
                throw new Exception("Patient not found in Edit Patient Profile Service");
            }

            patient.PatientPhones.Clear();

            var patientPhonesSpecification = new PatientPhonesWithFilterSpecification(patientId);

            var existingPatientPhones = await _unitOfWork.GetRepository<PatientPhone, string>().GetAllAsync(patientPhonesSpecification);

            _unitOfWork.GetRepository<PatientPhone, string>().DeleteAll(existingPatientPhones);

            _mapper.Map(updateRequest, patient);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task EditDoctorProfileService(DoctorDto updateRequest, string doctorId)
        {
            var specification = new DoctorWithFilterSpecification(doctorId);

            var doctor = await _unitOfWork.GetRepository<Doctor, string>().GetByIdAsync(specification);
            if (doctor == null)
            {
                throw new Exception("Doctor not found in Edit Soctor Profile Service");
            }

            doctor.DoctorPhones.Clear();

            var doctorPhonesSpecification = new DoctorPhonesWithFilterSpecification(doctorId);

            var existingDoctorPhones = await _unitOfWork.GetRepository<DoctorPhone, string>().GetAllAsync(doctorPhonesSpecification);

            _unitOfWork.GetRepository<DoctorPhone, string>().DeleteAll(existingDoctorPhones);

            _mapper.Map(updateRequest, doctor);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task EditAdminProfileService(AdminDto updateRequest, string adminId)
        {
            var specification = new AdminWithFilterSpecification(adminId);

            var admin = await _unitOfWork.GetRepository<Admin, string>().GetByIdAsync(specification);
            if (admin == null)
            {
                throw new Exception("Admin not found in Edit Soctor Profile Service");
            }

            admin.AdminPhones.Clear();

            var adminPhonesSpecification = new AdminPhonesWithFilterSpecification(adminId);

            var existingAdminPhones = await _unitOfWork.GetRepository<AdminPhone, string>().GetAllAsync(adminPhonesSpecification);

            _unitOfWork.GetRepository<AdminPhone, string>().DeleteAll(existingAdminPhones);

            _mapper.Map(updateRequest, admin);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
