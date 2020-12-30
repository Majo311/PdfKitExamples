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

    }
    public struct Role
    {
        public bool CanEdit { get; set; }
        public bool CanPrint { get; set; }
        public bool CanCopyOrExtract { get; set; }

     
    }
}
