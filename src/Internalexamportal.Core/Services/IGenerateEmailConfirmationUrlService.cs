namespace Internalexamportal.Core.Services
{
    public interface IGenerateEmailConfirmationUrlService
    {
        string GetEmailConfirmationUrl(string userId, string code);
    }
}
