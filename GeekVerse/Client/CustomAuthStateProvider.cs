using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace GeekVerse.Client
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _http;
        public CustomAuthStateProvider(ILocalStorageService LocalStorageService, HttpClient http)
        {
            _http = http;
            _localStorageService = LocalStorageService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var authToken = await _localStorageService.GetItemAsStringAsync("authToken");
            
            var identity = new ClaimsIdentity();
            //checar se essa secao Authorization esta sendo criada dentro da HTTP request !!!!!!!!!!!!!!
            _http.DefaultRequestHeaders.Authorization = null;

            //se for autenticado entra no if
            if(!string.IsNullOrEmpty(authToken))
            {
                try
                {
                    identity = new ClaimsIdentity(ParseFromClaimsJwt(authToken), "jwt");
                    //Header Auhorization nao esta sendo adcionada na api Login!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    //Console.WriteLine(authToken);
                    
                    _http.DefaultRequestHeaders.Authorization =
                       new AuthenticationHeaderValue("Bearer", authToken.Replace("\"", ""));
                    Console.WriteLine(_http.DefaultRequestHeaders.Authorization.Parameter);
                }
                catch
                {
                    await _localStorageService.RemoveItemAsync("authToken");
                    identity = new ClaimsIdentity();
                }
            }

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            //go to definition em ClaimsPrincipal para poder entender melhor.
            //bool status = state.User.Identity.IsAuthenticated;
            //bool status = new AuthenticationState(user).User.Identity.IsAuthenticated;
            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
               
            }

            //converte em array de ASCII
            return Convert.FromBase64String(base64);
        }

        private IEnumerable<Claim> ParseFromClaimsJwt(string jwt)
        {
            //pegando somente o payload do Token gerado para transformar em um IEnumerable de Claim
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            var claims = keyValuePairs.Select(kv => new Claim(kv.Key, kv.Value.ToString()));

            return claims;
        }
    }
}
