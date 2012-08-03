
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

    public class AccountController : Controller
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public IRepository<UserProfile> userRepository { get; set; } 

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            // need to do this beacuse even if we do signout there is still data available for this user in this get
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
            
            // Initial operation
            if (response == null)
            {
                Logger.Trace("Sending request to the open id provider");

                // Step 1 - Send the request to the OpenId provider server
                Identifier id;
                if (Identifier.TryParse(Request.Form["openid_identifier"], out id))
                {
                    try
                    {
                        var req = openid.CreateRequest(Request.Form["openid_identifier"]);

                        var fetch = new FetchRequest();
                        fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Contact.Email, true));
                        fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.Alias, true));
                        req.AddExtension(fetch);

                        return req.RedirectingResponse.AsActionResult();
                    }
                    catch (ProtocolException ex)
                    {
                        Logger.Error("Unable to authenticate: {0}", ex.Message);
                        this.ViewBag.Error = "Authentication failed";
                        return this.View("Login");
                    }
                }
                else
                {
                    this.ViewBag.Error = "Invalid identifier";
                    return this.View("Login");
                }
            }
            // OpenId redirection callback
            else
            {
                Logger.Trace("Received request from open id provider");

                // Step 2: OpenID Provider sending assertion response
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        Logger.Info("User succesfully authenticated");

                        // todo : create a simple parser of this data first tog et mail
                        var claimsResponse = response.GetExtension<FetchResponse>();
                        this.IssueAuthTicket(claimsResponse.Attributes[0].Values[0], true);

                        var currentUser = this.userRepository.GetByParameterEqualsFilter("UserId", claimsResponse.Attributes[0].Values[0]).SingleOrDefault();
                        if (currentUser == null)
                        {
                            Logger.Info("Detected new user. Creating new profile with UserId - {0}", claimsResponse.Attributes[0].Values[0]);
                            currentUser = new UserProfile
                                              {
                                                  UserId = claimsResponse.Attributes[0].Values[0],
                                                  Dashboards = new List<LearningDashboard>
                                                      {
                                                          new LearningDashboard()
                                                      }
                                              };
                            var id = this.userRepository.Add(currentUser);
                            if (id < 0)
                            {
                                Logger.Warn("Authentication procedure failed on adding new user, redirecting to Login");
                                this.ViewBag.Error = "Authentication failed";
                                return this.View("Login");  
                            }
                        }

                        SessionManager.CurrentUserSession = new UserProfileSession
                            {
                                ID = currentUser.ID, 
                                MainDashboardId = currentUser.Dashboards.First().ID,
                                UserId = currentUser.UserId
                            };

                        Logger.Info("Authentication procedure succesfull redirecting to Home/Index");
                        return this.RedirectToAction("Index", "Home");

                    case AuthenticationStatus.Canceled:
                        Logger.Warn("Authentication canceled : {0}",response.Exception.Message);
                        this.ViewBag.Error = "Authentication failed";
                        return this.View("Login");
                    case AuthenticationStatus.Failed:
                        Logger.Error("Authentication failed : {0}",response.Exception.Message);
                        this.ViewBag.Error = "Authentication failed";
                        return this.View("Login");
                }
            }
            return new EmptyResult();
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
