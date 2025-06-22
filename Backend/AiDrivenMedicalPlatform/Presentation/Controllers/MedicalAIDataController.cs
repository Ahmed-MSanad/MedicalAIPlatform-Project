using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.AiAnalysisDtos;
using Shared.PatientDtos;
using Shared;
using Shared.DoctorDtos;

namespace Presentation.Controllers
{
    public class MedicalAIDataController(IServiceManager serviceManager) : ApiController
    {
        [Authorize]
        [HttpPost]
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

        [Authorize]
        [HttpGet("{medicalImageId}")]
        public async Task<ActionResult<IEnumerable<AiAnalysisDto>>> GetMedicalImageAiAnalysis([FromRoute] int medicalImageId)
        {
            var aiAnalysis = await serviceManager.MedicalAIDataService.GetMedicalImageAiAnalysisService(medicalImageId);

            return Ok(aiAnalysis);
        }

        [Authorize]
        [HttpGet("{medicalImageId}")]
        public async Task<ActionResult<PatientDto>> GetMedicalImageOwner([FromRoute] int medicalImageId)
        {
            var patient = await serviceManager.MedicalAIDataService.GetMedicalImageOwnerService(medicalImageId);

            return Ok(patient);
        }

        [Authorize]
        [HttpGet("{doctorId}")]
        public async Task<ActionResult<DoctorDto>> GetAiAnalysisDoctorData([FromRoute] string doctorId)
        {
            var doctor = await serviceManager.MedicalAIDataService.GetAiAnalysisDoctorDataService(doctorId);

            return Ok(doctor);
        }
    }
}
