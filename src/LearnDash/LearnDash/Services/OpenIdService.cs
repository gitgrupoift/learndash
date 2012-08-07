namespace LearnDash.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    using DotNetOpenAuth.Messaging;
    using DotNetOpenAuth.OpenId;
    using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
    using DotNetOpenAuth.OpenId.RelyingParty;

    using LearnDash.Dal.Models;

    public static class OpenIdService
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static ActionResult SendRequest(OpenIdRelyingParty openid, Identifier openIdIdentifier)
        {
            Logger.Trace("Sending request to the open id provider");

            var req = openid.CreateRequest(openIdIdentifier);

            var fetch = new FetchRequest();
            fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Contact.Email, true));
            fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.Alias, true));
            req.AddExtension(fetch);
            return req.RedirectingResponse.AsActionResult();
        }

        public static string ParseResponse(IAuthenticationResponse response)
        {
            var claimsResponse = response.GetExtension<FetchResponse>();

            return claimsResponse.Attributes[0].Values[0];
        }
    }
}