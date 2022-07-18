namespace File_Sharing_App.Helper.Mail
{
    public interface IMailHelper
    {
        void SendMail(InputMailMessage model);
        void SendMailToAdmin(InputMailMessage model);
    }
}
