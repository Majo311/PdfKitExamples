using System;
using TallComponents.PDF.Security;

namespace pdfKitexamples
{
    public class SecurityManager : IDisposable
    {
        private PdfFile pdfFile;
        public EncryptionLevel EncryptionLevel { get; set; }
        public bool IsEncripted { get; private set; }
        public SecurityManager() :base()
        {
        }
        public SecurityManager(PdfFile PdfFile):base()
        {
            this.pdfFile = PdfFile;
            this.IsEncripted = PdfFile.Security == null ? false : true;
        }
        public void SetEcription(EncryptionLevel EncryptionLevel)
        {
            this.EncryptionLevel = EncryptionLevel;
        }
        public void AddPassword(User User)
        {
            if (!IsEncripted)
            {
                PasswordSecurity passwordSecurity = new PasswordSecurity();
                passwordSecurity.Change = User.Role.CanEdit;
                passwordSecurity.CopyExtract = User.Role.CanCopyOrExtract;
                passwordSecurity.Print = User.Role.CanPrint;
                passwordSecurity.EncryptionLevel = this.EncryptionLevel;
                if (User.IsCreator||User.IsOwner)
                {
                    passwordSecurity.UserPassword = User.Password;
                    this.pdfFile.Security = passwordSecurity;
                }
            }
        } 
        public void AddOwnerPassword(User User,User Owner)
        {
            if(User.IsCreator)
            {
                PasswordSecurity passwordSecurity = new PasswordSecurity();
                passwordSecurity.EncryptionLevel = this.EncryptionLevel;
                passwordSecurity.OwnerPassword = Owner.Password;//
                this.pdfFile.Security = passwordSecurity;
            }
        }
        public void RemovePassword(User User)
        {
            if(User.IsOwner||User.IsCreator)
            {
                this.pdfFile.Security = null;
            }
        }

        public void Dispose()
        {
            this.pdfFile.Dispose();
        }
    }
}


