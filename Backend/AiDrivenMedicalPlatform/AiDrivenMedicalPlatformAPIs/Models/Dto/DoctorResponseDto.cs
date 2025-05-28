using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AiDrivenMedicalPlatformAPIs.Models.Dto
{
    public class DoctorResponseDto
    {
        public string Id { get; set; }
        public string FullName { get; set; } 
        public string Specialisation { get; set; }
        public string WorkPlace { get; set; }
        public decimal Fee { get; set; }
        public decimal? Rate { get; set; }
        public byte[]? Image { get; set; }
    }
}
