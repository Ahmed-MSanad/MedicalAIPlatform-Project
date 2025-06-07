using Shared.FeedbackDtos;

namespace Services.Abstraction
{
    public interface IFeedbackService
    {
        public Task<FeedbackDto> SendPatientFeedbackAsync(FeedbackMessageDto feedbackMessageDto, string patientId);
        public Task<FeedbackDto> SendAdminResponse(FeedbackResponseDto responseDto, string adminId);
        public Task<IEnumerable<FeedbackDto>> GetAllFeedbacks();
        public Task<FeedbackDto> GetFeedBack(int feedbackId);
        public Task RemoveFeedBack(int feedbackId);
    }
}
