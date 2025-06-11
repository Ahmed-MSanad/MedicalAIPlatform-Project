using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.MedicalImageDtos
{
    public class MedicalImageDto
    {
        public int MedicalImageId { get; set; }
        public byte[] Image { get; set; } = null!;
    }
}
