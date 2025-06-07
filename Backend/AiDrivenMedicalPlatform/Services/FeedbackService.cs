using AutoMapper;
using MedicalProj.Data.Contracts;
using MedicalProj.Data.Models;
using Services.Abstraction;
using Shared.FeedbackDtos;

namespace Services
{
    public class FeedbackService(IUnitOfWork unitOfWork, IMapper mapper) : IFeedbackService
    {
        public async Task<FeedbackDto> SendPatientFeedbackAsync(FeedbackMessageDto feedbackMessageDto, string patientId)
        {
            var patientFeedback = mapper.Map<Feedback>(feedbackMessageDto);
            patientFeedback.PatientId = patientId;

            await unitOfWork.GetRepository<Feedback, int>().AddAsync(patientFeedback);

            var patientFeedbackDto = mapper.Map<FeedbackDto>(patientFeedback);

            await unitOfWork.SaveChangesAsync();

            return patientFeedbackDto;
        }

        public async Task<FeedbackDto> SendAdminResponse(FeedbackResponseDto responseDto, string adminId)
        {
            var feedbackToUpdate = await unitOfWork.GetRepository<Feedback, int>().GetByIdAsync(responseDto.FeedbackId);
            if (feedbackToUpdate == null)
            {
                return null!;
            }

            mapper.Map(responseDto, feedbackToUpdate);
            feedbackToUpdate.AdminId = adminId;

            unitOfWork.GetRepository<Feedback, int>().Update(feedbackToUpdate);

            var feedbackToUpdateDto = mapper.Map<FeedbackDto>(feedbackToUpdate);

            await unitOfWork.SaveChangesAsync();

            return feedbackToUpdateDto;
        }
    
        public async Task<IEnumerable<FeedbackDto>> GetAllFeedbacks()
        {
            var feedbacks = await unitOfWork.GetRepository<Feedback, int>().GetAllAsync();

            var feedbacksDtos = mapper.Map<IEnumerable<FeedbackDto>>(feedbacks);

            return feedbacksDtos;
        }

        public async Task<FeedbackDto> GetFeedBack(int feedbackId)
        {
            var feedback = await unitOfWork.GetRepository<Feedback, int>().GetByIdAsync(feedbackId);
            if (feedback == null)
            {
                return null!;
            }
            return mapper.Map<FeedbackDto>(feedback);
        }
    
        public async Task RemoveFeedBack(int feedbackId)
        {
            var feedback = await unitOfWork.GetRepository<Feedback, int>().GetByIdAsync(feedbackId);
            if (feedback != null)
            {
                unitOfWork.GetRepository<Feedback, int>().Delete(feedback);
            }
            else
            {
                throw new KeyNotFoundException($"Feedback with ID {feedbackId} not found to be deleted.");
            }

            await unitOfWork.SaveChangesAsync();
        }
    }
}
