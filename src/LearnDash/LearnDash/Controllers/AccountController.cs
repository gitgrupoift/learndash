
namespace LearnDash.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Web;
    using System.Web.ClientServices;
    using System.Web.Mvc;
    using System.Web.Security;
    using Castle.Core.Logging;
    using DotNetOpenAuth.Messaging;
    using DotNetOpenAuth.OAuth.ChannelElements;
    using DotNetOpenAuth.OpenId;
    using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
    using DotNetOpenAuth.OpenId.RelyingParty;
    using LearnDash.Dal.Models;
    using LearnDash.Dal.NHibernate;
    using LearnDash.Services;

    public class AccountController : Controller
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly string SpecialHiddenPassword = "alphatest";

        public IRepository<UserProfile> UserRepository { get; set; }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            // need to do this beacuse even if we do signout there is still data available for this user in context
            HttpContext.User = null;

            Logger.Trace("User Logged out");
            return this.View();
        }

        [Authorize]
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult Login()
        {
            Logger.Trace("Login Action");
            return this.View();
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get), ValidateInput(false)]
        public ActionResult OpenIdLogOn()
        {

                var openid = new OpenIdRelyingParty();
                var response = openid.GetResponse();

                if (response == null)
                {
                    var password = this.Request.Form["passwordBox"];
                    if (password == SpecialHiddenPassword)
                    {
                        return this.OpenIdRequest(openid);
                    }
                    else
                    {
                        Logger.Error("Wrong password in alpha version");
                        this.ViewBag.Error = "Incorrect password please contact us if you want to participate in alpha tests.";
                        return this.View("Login");
                    }
                }
                else
                {
                    return this.OpenIdResponse(response);
                }
        }

        private ActionResult OpenIdResponse(IAuthenticationResponse response)
        {
            Logger.Trace("Received response from open id provider");

            // Step 2: OpenID Provider sending assertion response
            switch (response.Status)
            {
                case AuthenticationStatus.Authenticated:
                    Logger.Info("User succesfully authenticated");

                    var userId = OpenIdService.ParseResponse(response);
                    var currentUser = this.UserRepository.GetByParameterEqualsFilter("UserId", userId).SingleOrDefault();
                    if (currentUser == null)
                    {
                        Logger.Info("New user. Creating new profile with UserId - {0}", userId);
                        currentUser = new UserProfile
                            {
                                UserId = userId,
                                Dashboards = new List<LearningDashboard>()
                            };

                        var id = this.UserRepository.Add(currentUser);
                        if (id < 0)
                        {
                            Logger.Warn("Authentication procedure failed on adding new user, redirecting to Login");
                            this.ViewBag.Error = "Authentication failed";
                            return this.View("Login");
                        }
                    }

                    this.IssueAuthTicket(currentUser.UserId, true);

                    SessionManager.CurrentUserSession = new UserProfileSession
                        {
                            ID = currentUser.ID,
                            MainDashboardId = currentUser.Dashboards.First().ID,
                            UserId = currentUser.UserId
                        };

                    Logger.Info("Authentication procedure succesfull redirecting to Home/Index");
                    return this.RedirectToAction("Index", "Home");

                case AuthenticationStatus.Canceled:
                case AuthenticationStatus.Failed:
                    Logger.Error("Authentication failed : {0}", response.Exception.Message);
                    this.ViewBag.Error = "Authentication failed";
                    return this.View("Login");
            }

            return new EmptyResult();
        }

        private ActionResult OpenIdRequest(OpenIdRelyingParty openid)
        {
            var openIdIdentifier = this.Request.Form["openid_identifier"];
            Identifier id;
            if (Identifier.TryParse(openIdIdentifier, out id))
            {
                try
                {
                    return OpenIdService.SendRequest(openid, id);
                }
                catch (ProtocolException ex)
                {
                    Logger.Error("OpenIdRequestError: {0}", ex.Message);
                    this.ViewBag.Error = "Authentication failed";
                    return this.View("Login");
                }
            }
            else
            {
                Logger.Error("Openid_identifier parse error.\r\nThis value should be in the Form Request.\r\nPossible error in View.");
                this.ViewBag.Error = "Authentication error";
                return this.View("Login");
            }
        }

        private void IssueAuthTicket(string userId, bool rememberMe)
        {
            var ticket = new FormsAuthenticationTicket(
                1, userId, DateTime.Now, DateTime.Now.AddDays(10), rememberMe, userId);

            string ticketString = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticketString);

            if (rememberMe)
            {
                cookie.Expires = DateTime.Now.AddDays(10);
            }

            HttpContext.Response.Cookies.Add(cookie);
            Logger.Info("Authentication Ticket Created");
        }
    }
}
