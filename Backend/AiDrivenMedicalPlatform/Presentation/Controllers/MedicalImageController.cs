using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.MedicalImageDtos;

namespace Presentation.Controllers
{
    [Authorize]
    [Route("api")]
    public class MedicalImageController(IServiceManager serviceManager) : ApiController
    {
        [HttpPost("medical-images")]
        public async Task<ActionResult> AddMedicalImage([FromBody] CreatedMedicalImageDto medicalImageDto)
        {
            string patientId = User.Claims.First(x => x.Type == "UserID").Value;

            await serviceManager.MedicalImageService.AddMedicalImageService(medicalImageDto, patientId);

            return StatusCode(StatusCodes.Status201Created, new { message = "Image Added Successfully" });
        }

        [HttpPatch("medical-images/{medicalImageId:int}")]
        public async Task<ActionResult> EditMedicalImage([FromRoute] int medicalImageId, [FromBody] NewImageDto newImageDto)
        {
            await serviceManager.MedicalImageService.EditMedicalImageService(medicalImageId,newImageDto);

            return Ok(new { message = "Image Updated Successfully" });
        }

        [HttpDelete("medical-images/{medicalImageId:int}")]
        public async Task<ActionResult> DeleteMedicalImage([FromRoute] int medicalImageId)
        {
            await serviceManager.MedicalImageService.DeleteMedicalImageService(medicalImageId);

            return Ok(new { message = "Image Deleted Successfully" });
        }

        [HttpGet("appointments/{appointmentId:int}/medical-images")]
        public async Task<ActionResult> GetMedicalImage([FromRoute] int appointmentId)
        {
            var medicalImage = await serviceManager.MedicalImageService.GetMedicalImageService(appointmentId);

            return Ok(medicalImage);
        }
    }
}