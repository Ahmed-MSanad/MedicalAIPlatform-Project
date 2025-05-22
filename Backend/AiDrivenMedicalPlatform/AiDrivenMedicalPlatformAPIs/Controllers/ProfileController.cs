using AiDrivenMedicalPlatformAPIs.Models.Dto;
using MedicalProj.Data.Contexts;
using MedicalProj.Data.Models;
using MedicalProj.Data.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AiDrivenMedicalPlatformAPIs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly MedicalDbContext context;
        private readonly UserManager<AppUser> userManager;

        public ProfileController(MedicalDbContext _context, UserManager<AppUser> _userManager)
        {
            context = _context;
            userManager = _userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PatientDto>> GetPatientProfile()
        {
            string userId = User.Claims.First(x => x.Type == "UserID").Value;
            var userDetails = await context.Patients.Include(p => p.PatientPhones).FirstOrDefaultAsync(p => p.Id == userId);

            if (userDetails == null)
            {
                return NotFound("Patient not found.");
            }

            return Ok(
                new
                {
                    Email = userDetails?.Email,
                    FullName = userDetails?.FullName,
                    DateOfBirth = userDetails?.DateOfBirth,
                    Gender = userDetails?.Gender,
                    Address = userDetails?.Address,
                    Occupation = userDetails?.Occupation,
                    PatientPhones = userDetails?.PatientPhones.Select(phone => phone.Phone).ToList(),
                    EmergencyContactName = userDetails?.EmergencyContactName,
                    EmergencyContactNumber = userDetails?.EmergencyContactNumber,
                    FamilyMedicalHistory = userDetails?.FamilyMedicalHistory,
                    PastMedicalHistory = userDetails?.PastMedicalHistory,
                    Image = userDetails?.Image,
                }
            );
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<DoctorDto>> GetDoctorProfile()
        {
            string userId = User.Claims.First(x => x.Type == "UserID").Value;
            var userDetails = await context.Doctors.Include(d => d.DoctorPhones).FirstOrDefaultAsync(d => d.Id == userId);
            return Ok(
                new
                {
                    Email = userDetails?.Email,
                    FullName = userDetails?.FullName,
                    DateOfBirth = userDetails?.DateOfBirth,
                    Gender = userDetails?.Gender,
                    Address = userDetails?.Address,
                    DoctorPhones = userDetails?.DoctorPhones.Select(p => p.Phone).ToList(),
                    Image = userDetails?.Image,
                    IdentificationNumber = userDetails?.IdentificationNumber,
                    MedicalLicenseNumber = userDetails?.MedicalLicenseNumber,
                    Specialisation = userDetails?.Specialisation,
                    WorkPlace = userDetails?.WorkPlace,
                }
            );
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<AdminDto>> GetAdminProfile()
        {
            string userId = User.Claims.First(x => x.Type == "UserID").Value;
            var userDetails = await context.Admins.Include(p => p.AdminPhones).FirstOrDefaultAsync(a => a.Id == userId);
            return Ok(
                new
                {
                    Email = userDetails?.Email,
                    FullName = userDetails?.FullName,
                    DateOfBirth = userDetails?.DateOfBirth,
                    Gender = userDetails?.Gender,
                    Address = userDetails?.Address,
                    AdminPhones = userDetails?.AdminPhones,
                    Image = userDetails?.Image,
                    IdentificationNumber = userDetails?.IdentificationNumber,
                    MedicalLicenseNumber = userDetails?.MedicalLicenseNumber,
                    Specialisation = userDetails?.Specialisation,
                }
            );
        }


        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteUserProfile()
        {
            string userId = User.Claims.First(x => x.Type == "UserID").Value;
            var userDetails = await userManager.FindByIdAsync(userId);
            if (userDetails == null) return NotFound("User not found");

            var result = await userManager.DeleteAsync(userDetails);
            if (result.Succeeded)
            {
                return Ok("User deleted successfully");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }


        [Authorize]
        [HttpPut]
        public async Task<ActionResult> EditPatientProfile([FromBody] PatientDto updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserID");
            if (userIdClaim == null)
            {
                return Unauthorized("User ID claim not found in token.");
            }
            string userId = userIdClaim.Value;


            var patient = await context.Patients.FirstOrDefaultAsync(p => p.Id == userId);
            if (patient == null)
            {
                return NotFound("Patient profile not found");
            }


            patient.FullName = updateRequest.FullName;
            patient.DateOfBirth = updateRequest.DateOfBirth;
            patient.Gender = Enum.IsDefined(typeof(Gender), updateRequest.Gender) ? (Gender)updateRequest.Gender : patient.Gender;
            patient.Address = updateRequest.Address;
            patient.Occupation = updateRequest.Occupation;
            patient.EmergencyContactName = updateRequest.EmergencyContactName;
            patient.EmergencyContactNumber = updateRequest.EmergencyContactNumber;
            patient.FamilyMedicalHistory = updateRequest.FamilyMedicalHistory;
            patient.PastMedicalHistory = updateRequest.PastMedicalHistory;

            try // this works for testing on swagger -> if not over the front end we'll return it back to byte[]
            {
                patient.Image = string.IsNullOrEmpty(updateRequest.Image)
                    ? null
                    : Convert.FromBase64String(updateRequest.Image);
            }
            catch (FormatException)
            {
                ModelState.AddModelError("Image", "Invalid base64 string for image.");
                return BadRequest(ModelState);
            }

            patient.PatientPhones.Clear();
            patient.PatientPhones = updateRequest.PatientPhones.Select(phone => new PatientPhone
            {
                Phone = phone
            }).ToList();

            try
            {
                await context.SaveChangesAsync();
                return Ok("Profile updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update profile: {ex.Message}");
            }
        }


        [Authorize]
        [HttpPut]
        public async Task<ActionResult> EditDoctorProfile([FromBody] DoctorDto updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserID");
            if (userIdClaim == null)
            {
                return Unauthorized("User ID claim not found in token.");
            }
            string userId = userIdClaim.Value;


            var doctor = await context.Doctors.FirstOrDefaultAsync(d => d.Id == userId);
            if (doctor == null)
            {
                return NotFound("User not found");
            }

            doctor.FullName = updateRequest.FullName;
            doctor.DateOfBirth = updateRequest.DateOfBirth;
            doctor.Gender = (Gender)updateRequest.Gender;
            doctor.Address = updateRequest.Address;
            doctor.DoctorPhones.Clear();
            doctor.DoctorPhones = updateRequest.DoctorPhones.Select(phone => new DoctorPhone
            {
                Phone = phone
            }).ToList();
            doctor.IdentificationNumber = updateRequest.IdentificationNumber;
            doctor.MedicalLicenseNumber = updateRequest.MedicalLicenseNumber;
            doctor.Specialisation = updateRequest.Specialisation;
            doctor.WorkPlace = updateRequest.WorkPlace;


            try // this works for testing on swagger -> if not over the front end we'll return it back to byte[]
            {
                doctor.Image = string.IsNullOrEmpty(updateRequest.Image)
                    ? null
                    : Convert.FromBase64String(updateRequest.Image);
            }
            catch (FormatException)
            {
                ModelState.AddModelError("Image", "Invalid base64 string for image.");
                return BadRequest(ModelState);
            }

            try
            {
                await context.SaveChangesAsync();
                return Ok("Profile updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update profile: {ex.Message}");
            }
        }


        [Authorize]
        [HttpPut]
        public async Task<ActionResult> EditAdminProfile([FromBody] AdminDto updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserID");
            if (userIdClaim == null)
            {
                return Unauthorized("User ID claim not found in token.");
            }
            string userId = userIdClaim.Value;


            var admin = await context.Admins.FirstOrDefaultAsync(a => a.Id == userId);
            if (admin == null)
            {
                return NotFound("Patient profile not found");
            }


            admin.FullName = updateRequest.FullName;
            admin.DateOfBirth = updateRequest.DateOfBirth;
            admin.Gender = Enum.IsDefined(typeof(Gender), updateRequest.Gender) ? (Gender)updateRequest.Gender : admin.Gender;
            admin.Address = updateRequest.Address;
            admin.IdentificationNumber = updateRequest.IdentificationNumber;
            admin.MedicalLicenseNumber = updateRequest.MedicalLicenseNumber;
            admin.Specialisation = updateRequest.Specialisation;

            try // this works for testing on swagger -> if not over the front end we'll return it back to byte[]
            {
                admin.Image = string.IsNullOrEmpty(updateRequest.Image)
                    ? null
                    : Convert.FromBase64String(updateRequest.Image);
            }
            catch (FormatException)
            {
                ModelState.AddModelError("Image", "Invalid base64 string for image.");
                return BadRequest(ModelState);
            }

            admin.AdminPhones.Clear();
            admin.AdminPhones = updateRequest.AdminPhones.Select(phone => new AdminPhone
            {
                Phone = phone
            }).ToList();

            try
            {
                await context.SaveChangesAsync();
                return Ok("Profile updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update profile: {ex.Message}");
            }

        }
    
    }
}
