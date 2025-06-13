using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.MedicalImageDtos
{
    public class CreatedMedicalImageDto
    {
        public byte[] Image { get; set; } = null!;
        public DateTime UploadDate { get; set; } = DateTime.Now;
        public string Did { get; set; } = null!;
        public int AppointmentId { get; set; }
    }
}
