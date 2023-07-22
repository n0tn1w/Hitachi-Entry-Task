using HitachiBE.Models.Request;

namespace HitachiBE.Services
{
    public interface ISpaceService
    {
        //Task UploadFile(IFormFile file);

        Task SendEmail(EmailDataRequest input);
    }
}
