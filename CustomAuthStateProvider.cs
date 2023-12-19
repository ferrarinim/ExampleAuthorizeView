using ExampleAuthorizeView.Data;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExampleAuthorizeView
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthStateProvider(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public async Task<Utenti?> GetUser()
        {
            var userSessionStorageResult = await _sessionStorage.GetAsync<Utenti>("user");
            return userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                Utenti? userSession = await GetUser();
                if (userSession == null)
                {
                    return await Task.FromResult(new AuthenticationState(_anonymous));
                }
                else
                {
                    var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {

                    new Claim("IdUser", userSession.IdUtente.ToString()),
                    new Claim("UserStatus", userSession.Stato),
                    new Claim(ClaimTypes.Name, userSession?.Nome ?? String.Empty),
                    new Claim(ClaimTypes.Email, userSession?.email ?? String.Empty),
                    new Claim(ClaimTypes.NameIdentifier, userSession.IdUtente.ToString())
                }, "CustomAuth"));

                    return await Task.FromResult(new AuthenticationState(claimsPrincipal));
                }
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }

        public async Task UpdateAuthenticationState(Utenti parUser)
        {
            ClaimsPrincipal claimsPrincipal;
            if (parUser != null)
            {
                await _sessionStorage.SetAsync("user", parUser);
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim("IdUser", parUser.IdUtente.ToString()),
                    new Claim("UserStatus", parUser.Stato),
                    new Claim(ClaimTypes.Name, parUser.Nome),
                    new Claim(ClaimTypes.Email, parUser.email),
                    new Claim(ClaimTypes.NameIdentifier, parUser.IdUtente.ToString())
                }));
            }
            else
            {
                await _sessionStorage.DeleteAsync("user");
                claimsPrincipal = _anonymous;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
            
        }


        
    }
}
