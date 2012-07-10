using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Castle.Core.Logging;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;
using LearnDash.Dal.Models;
using LearnDash.Dal.NHibernate;

namespace LearnDash.Controllers
{
    public class AccountController : Controller
    {
        public static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public IRepository<UserProfile> _userRepository { get; set; } 

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return View();
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get), ValidateInput(false)]
        public ActionResult OpenIdLogOn(string returnUrl)
        {
            var openid = new OpenIdRelyingParty();
            var response = openid.GetResponse();
            if (response == null)  // Initial operation
            {
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
                        this.ViewBag.Error = ("Unable to authenticate: " + ex.Message);
                        return View("Login");
                    }
                }
                else
                {
                    this.ViewBag.Error = ("Invalid identifier");
                    return View("Login");
                }
            }
            else  // OpenId redirection callback
            {
                // Step 2: OpenID Provider sending assertion response
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:

                        //todo : create a simple parser of this data first tog et mail
                        var claimsResponse = response.GetExtension<FetchResponse>();
                        this.IssueAuthTicket(claimsResponse.Attributes[0].Values[0], true);

                        var currentUser = _userRepository.GetByParameterEqualsFilter("UserId", claimsResponse.Attributes[0].Values[0]).SingleOrDefault();
                        if (currentUser == null)
                        {
                            Logger.Info("Detected new user. Creating new profile with UserId - {0}", claimsResponse.Attributes[0].Values[0]);
                            currentUser = new UserProfile
                                              {
                                                  UserId = claimsResponse.Attributes[0].Values[0],
                                                  Dashboards = new List<LearningDashboard> {new LearningDashboard()}
                                              };
                            _userRepository.Add(currentUser);
                        }

                        SessionManager.CurrentUser = currentUser;

                        if (!string.IsNullOrEmpty(returnUrl))
                            return Redirect(returnUrl);

                        return RedirectToAction("Index","Home");

                    case AuthenticationStatus.Canceled:
                        this.ViewBag.Error = ("Canceled at provider");
                        return View("Login");
                    case AuthenticationStatus.Failed:
                        this.ViewBag.Error = (response.Exception.Message);
                        return View("Login");
                }
            }
            return new EmptyResult();
        }

        private void IssueAuthTicket(string userId, bool rememberMe)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userId,
                                                                 DateTime.Now, DateTime.Now.AddDays(10),
                                                                 rememberMe, userId);

            string ticketString = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticketString);
            if (rememberMe)
                cookie.Expires = DateTime.Now.AddDays(10);

            HttpContext.Response.Cookies.Add(cookie);
        }
    }
}
