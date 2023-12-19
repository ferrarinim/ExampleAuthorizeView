using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExampleAuthorizeView.Shared
{
    public partial class StickyMenu : IDisposable
    {
        [CascadingParameter] private Task<AuthenticationState> authTask { get; set; } = default!;
        [Inject] private AuthenticationStateProvider authState { get; set; } = default!;


        private CustomAuthStateProvider AuthState => (CustomAuthStateProvider)authState!;

        private ClaimsPrincipal user = new ClaimsPrincipal();
        private string _currentUserName = "None";

        protected async override Task OnInitializedAsync()
        {
            ArgumentNullException.ThrowIfNull(authTask);
            var state = await authTask;
            this.user = state.User;
            authState.AuthenticationStateChanged += this.OnUserChanged;
        }


        private async void OnUserChanged(Task<AuthenticationState> state)
            => await this.GetUser(state);

        private async Task GetUser(Task<AuthenticationState> state)
        {
            var authState = await state;
            this.user = authState.User;
            await this.InvokeAsync(this.StateHasChanged);
        }

        public void Dispose()
            => authState.AuthenticationStateChanged -= this.OnUserChanged;

        private bool collapseNavMenu = true;

        private string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}
