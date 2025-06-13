using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.MedicalImageDtos;

namespace Presentation.Controllers
{
    public class MedicalImageController(IServiceManager serviceManager) : ApiController
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddMedicalImage([FromBody] CreatedMedicalImageDto medicalImageDto)
        {
            string patientId = User.Claims.First(x => x.Type == "UserID").Value;

            await serviceManager.MedicalImageService.AddMedicalImageService(medicalImageDto, patientId);

            return StatusCode(StatusCodes.Status201Created, new { Message = "Image Added Successfully" });
        }

        [Authorize]
        [HttpPatch]
        public async Task<ActionResult> EditMedicalImage([FromQuery] int medicalImageId,[FromBody] NewImageDto newImageDto)
        {
            await serviceManager.MedicalImageService.EditMedicalImageService(medicalImageId,newImageDto);

            return StatusCode(StatusCodes.Status200OK, new { Message = "Image Updated Successfully" });
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteMedicalImage([FromQuery] int medicalImageId)
        {
            await serviceManager.MedicalImageService.DeleteMedicalImageService(medicalImageId);

            return StatusCode(StatusCodes.Status200OK, new { Message = "Image Deleted Successfully" });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetMedicalImage([FromQuery] int appointmentId)
        {
            var medicalImage = await serviceManager.MedicalImageService.GetMedicalImageService(appointmentId);

            return Ok(medicalImage);
        }
    }
}
