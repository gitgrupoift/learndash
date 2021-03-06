﻿using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;

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
                return base.GetControllerInstance(requestContext, controllerType);
            }

            return (IController)kernel.Resolve(controllerType);
        }
    }
}