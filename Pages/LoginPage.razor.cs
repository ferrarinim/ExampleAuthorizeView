using ExampleAuthorizeView.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace ExampleAuthorizeView.Pages
{
    public partial class LoginPage : BaseComponent
    {
        [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }


        public async void LoginAutomatically()
        {
            await Task.Run(() =>
            {
                if (__idUser == 0)
                {
                    Utenti ut = Utenti.Login("email", "password");
                    if (ut.IdUtente > 0)
                    {
                       // var customAuthStateProvider = (CustomAuthStateProvider)AuthenticationStateProvider;
                      //  customAuthStateProvider.UpdateAuthenticationState(ut);
                        //NavigationManager.NavigateTo(Rewrite.Chiave, true);

                         _ = SetLoggedUser(ut);
                    }
                }

            });

            StateHasChanged();
        }
    }
}
