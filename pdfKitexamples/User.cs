using System;


namespace pdfKitexamples
{
    public class User
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public bool IsOwner { get; set; } = false;
        public bool IsCreator { get; set; } = false;
        public Role Role { get; set; }
        public User(string Password, bool IsOwner = false, bool IsCreator = true, Role Role =null)
        {
            this.Password = Password;
            this.IsOwner = IsOwner;
            this.IsCreator = IsCreator;
            this.Role = Role==null?new Role():Role;
        }

    }
    public class Role
    {
        public int Id { get; set; } = 0;
        public bool CanEdit { get; set; } = false;
        public bool CanPrint { get; set; } = false;
        public bool CanCopyOrExtract { get; set; } = false;
        public Role()
        {

        }
   
    }
}
