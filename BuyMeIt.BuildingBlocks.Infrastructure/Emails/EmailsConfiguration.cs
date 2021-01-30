namespace BuyMeIt.BuildingBlocks.Infrastructure.Emails
{
    public struct EmailsConfiguration
    {
        public EmailsConfiguration(string fromEmail)
        {
            FromEmail = fromEmail;
        }

        public string FromEmail { get; }
    }
}
