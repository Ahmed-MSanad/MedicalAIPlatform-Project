using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.AiAnalysisDtos;
using Shared.PatientDtos;
using Shared;
using Shared.DoctorDtos;

namespace Presentation.Controllers
{
    [Authorize]
    public class MedicalAIDataController(IServiceManager serviceManager) : ApiController
    {
        [HttpPost("medical-images/analysis")]
        public async Task<ActionResult> SetMedicalImageAiAnalysis([FromBody] AiAnalysisDto aiAnalysisDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = ModelState });
            }
            try
            {
                string patientId = await serviceManager.MedicalAIDataService.SetMedicalImageAiAnalysisService(aiAnalysisDto);

                await serviceManager.NotificationService.SendEmailToPatient(NotificationType.Alert, patientId);

                return Ok(new { message = "AI analysis set successfully, and notification Email is sent to the patient" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("medical-images/{medicalImageId}/analysis")]
        public async Task<ActionResult<IEnumerable<AiAnalysisDto>>> GetMedicalImageAiAnalysis([FromRoute] int medicalImageId)
        {
            var aiAnalysis = await serviceManager.MedicalAIDataService.GetMedicalImageAiAnalysisService(medicalImageId);

            return Ok(aiAnalysis);
        }

        [HttpGet("medical-images/{medicalImageId}/patient")]
        public async Task<ActionResult<PatientDto>> GetMedicalImageOwner([FromRoute] int medicalImageId)
        {
            var patient = await serviceManager.MedicalAIDataService.GetMedicalImageOwnerService(medicalImageId);

            return Ok(patient);
        }

        [HttpGet("doctors/{doctorId}/ai-data")]
        public async Task<ActionResult<DoctorDto>> GetAiAnalysisDoctorData([FromRoute] string doctorId)
        {
            var doctor = await serviceManager.MedicalAIDataService.GetAiAnalysisDoctorDataService(doctorId);

            return Ok(doctor);
        }
    }
}
