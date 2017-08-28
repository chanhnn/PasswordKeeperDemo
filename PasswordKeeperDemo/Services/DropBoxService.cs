using System;
using System.IO;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using PasswordKeeperDemo.Models;
using Xamarin.Forms;

namespace PasswordKeeperDemo.Services
{
    public class DropBoxService
    {
        #region Constants

        private const string AppKeyDropboxtoken = "YN7W1pDRmkEAAAAAAAAAk5AKnX4ADeNTrkLXldXGMu2Mkk14eSlUgdXFV3FMnKso";

        private const string ClientId = "mm1ox8efjk5cr5a";

        private const string RedirectUri = "https://www.dropbox.com/login";

        #endregion

        #region Fields

        /// <summary>
        ///     Occurs when the user was authenticated
        /// </summary>
        public Action OnAuthenticated;

        private string oauth2State;

        #endregion

        #region Properties

        private string AccessToken { get; set; }

        #endregion

        #region Public Methods and Operators

        public async Task VerifyAuthorize()
        {
            if (string.IsNullOrWhiteSpace(this.AccessToken) == false)
            {
                // Already authorized
                this.OnAuthenticated?.Invoke();
                return;
            }

            if (this.GetAccessTokenFromSettings())
            {
                // Found token and set AccessToken 
                return;
            }
        }

        public async Task Authorize()
        {
            if (string.IsNullOrWhiteSpace(this.AccessToken) == false)
            {
                // Already authorized
                this.OnAuthenticated?.Invoke();
                return;
            }

            if (this.GetAccessTokenFromSettings())
            {
                // Found token and set AccessToken 
                return;
            }

            // Run Dropbox authentication
            this.oauth2State = Guid.NewGuid().ToString("N");
            var authorizeUri = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, ClientId, new Uri(RedirectUri), this.oauth2State);
            var webView = new WebView { Source = new UrlWebViewSource { Url = authorizeUri.AbsoluteUri } };
            webView.Navigating += this.WebViewOnNavigating;
            var contentPage = new ContentPage { Content = webView };
            await Application.Current.MainPage.Navigation.PushModalAsync(contentPage);
        }

        public async Task<DropboxInfo> GetDropboxInfo()
        {
            using (var client = this.GetClient())
            {
                var user = await client.Users.GetCurrentAccountAsync();
                var displayName = user.Name.DisplayName;
                var email = user.Email;

                return new DropboxInfo
                {
                    DisplayName = displayName,
                    Email = email
                };
            }
        }

        public async Task BackupFile(string fileName)
        {
            //using (var client = new DropboxClient(this.AccessToken))
            //{
            //    // Write file					
            //    var commitInfo = new CommitInfo(fileName, WriteMode.Overwrite.Instance, false, DateTime.Now);
            //    var metadata = await client.Files.UploadAsync(commitInfo, new MemoryStream(fileContent));
            //}
        }

        public async Task<FileMetadata> WriteFile(byte[] fileContent, string filename)
        {
            try
            {
                using (var client = this.GetClient())
                {
                    // Write file					
                    var commitInfo = new CommitInfo(filename, WriteMode.Overwrite.Instance, false, DateTime.Now);
                    return await client.Files.UploadAsync(commitInfo, new MemoryStream(fileContent));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<byte[]> ReadFile(string file)
        {
            try
            {
                using (var client = this.GetClient())
                {
                    var response = await client.Files.DownloadAsync(file);
                    var bytes = response?.GetContentAsByteArrayAsync();
                    return bytes?.Result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Methods

        private DropboxClient GetClient()
        {
            return new DropboxClient(this.AccessToken);
        }

        /// <summary>
        ///     Tries to find the Dropbox token in application settings
        /// </summary>
        /// <returns>Token as string or <c>null</c></returns>
        private bool GetAccessTokenFromSettings()
        {
            try
            {
                if (!Application.Current.Properties.ContainsKey(AppKeyDropboxtoken))
                {
                    return false;
                }

                this.AccessToken = Application.Current.Properties[AppKeyDropboxtoken]?.ToString();
                if (this.AccessToken != null)
                {
                    this.OnAuthenticated.Invoke();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async void WebViewOnNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (!e.Url.StartsWith(RedirectUri, StringComparison.OrdinalIgnoreCase))
            {
                // we need to ignore all navigation that isn't to the redirect uri.
                return;
            }

            try
            {
                var result = DropboxOAuth2Helper.ParseTokenFragment(new Uri(e.Url));

                if (result.State != this.oauth2State)
                {
                    return;
                }

                this.AccessToken = result.AccessToken;

                await SaveDropboxToken(this.AccessToken);
                this.OnAuthenticated?.Invoke();
            }
            catch (ArgumentException)
            {
                // There was an error in the URI passed to ParseTokenFragment
            }
            finally
            {
                e.Cancel = true;
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        /// <summary>
        ///     Saves the Dropbox token to app settings
        /// </summary>
        /// <param name="token">Token received from Dropbox authentication</param>
        private static async Task SaveDropboxToken(string token)
        {
            if (token == null)
            {
                return;
            }

            try
            {
                Application.Current.Properties.Add(AppKeyDropboxtoken, token);
                await Application.Current.SavePropertiesAsync();
            }
            catch (Exception ex)
            {
            }
        }

        #endregion
    }
}
