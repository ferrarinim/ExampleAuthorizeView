using ExampleAuthorizeView.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace ExampleAuthorizeView
{
    public class BaseComponent : LayoutComponentBase
    {
        #region Inject
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        #endregion

        //  private int __Id = null;
        private Utenti loggedUser = null;
        protected Utenti LoggedUser
        {
            get
            {
                if (loggedUser == null)
                {
                    var customAuthStateProvider = (CustomAuthStateProvider)AuthenticationStateProvider;
                    loggedUser = customAuthStateProvider.GetUser()?.Result;

                }

                return loggedUser;
            }
        }

        

        public int __idUser => LoggedUser?.IdUtente ?? 0;
        public string __name => LoggedUser == null ? "" : (LoggedUser?.Stato=="TMP" ? "OSPITE": LoggedUser?.Nome);


        public bool IsLoading { get; set; } = false;


        public async Task SetLoggedUser(Utenti u, string url=null)
        {
            loggedUser = u;
            var customAuthStateProvider = (CustomAuthStateProvider)AuthenticationStateProvider;
            await customAuthStateProvider.UpdateAuthenticationState(u);
            if (url != null)
            {
                NavigationManager.NavigateTo(url, true);
            }
        }

        public void NavigateTo(string url)
        {
            NavigationManager.NavigateTo(url);
        }

        public void NavigateToNewTab(string url)
        {
            JSRuntime.InvokeAsync<object>("open", url, "_blank");
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }


        public void StartLoading()
        {
            IsLoading = true;
            Task.Delay(1);
        }

        public void EndLoading()
        {
            IsLoading = false;
            Task.Delay(1);
        }


    }
}
