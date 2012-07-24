using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearnDash.Controllers
{
    public enum NotificationType
    {
        SuccesfullyAdd,
        SuccesfullyDeleted,
        SuccesfullyEdited,
        FailAdd,
        FailDeleted,
        FailEdited,
        FailedToLoadFlow,
        blank
    }
}