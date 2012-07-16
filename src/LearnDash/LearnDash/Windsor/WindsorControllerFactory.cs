using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Core.Logging;
using Castle.MicroKernel;
using Core.Extensions;

namespace LearnDash.Windsor
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        public static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IKernel kernel;

        public WindsorControllerFactory(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public override void ReleaseController(IController controller)
        {
            kernel.ReleaseComponent(controller);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                var message = string.Format("The controller for path '{0}' could not be found.", requestContext.HttpContext.Request.Path);
                Logger.Error(message);
                throw new HttpException(message);
            }

            return (IController)kernel.Resolve(controllerType);
        }
    }
}