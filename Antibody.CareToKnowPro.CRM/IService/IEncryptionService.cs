namespace Antibody.CareToKnowPro.CRM.IService
{
    public interface IEncryptionService
    {
        string EncryptPassword(string clearPassword);
        string DecryptPassword(string encryptedPassword);
    }
}