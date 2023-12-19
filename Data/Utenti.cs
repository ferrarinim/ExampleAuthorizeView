

namespace ExampleAuthorizeView.Data
{
    public partial class Utenti
    {

        public int IdUtente { get; set; }

        public string? Nome { get; set; }


        public string? email { get; set; }

        public string? password { get; set; }


        public string? Stato { get; set; }

        public static Utenti Login(string username, string password)
        {
            Utenti u = new Utenti();
            u.email = username;
            u.password = password;
            u.IdUtente = 100;
            u.Stato = "INS";
            u.Nome = "User Logged IN Automatically";
            return u;
        }


    }
}
