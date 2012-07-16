using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearnDash.Controllers
{
    public enum NotificationType
    {
        Succesfully_add,
        Succesfully_deleted,
        Succesfully_edited,
        Fail_add,
        Fail_deleted,
        Fail_edited,
        Failed_to_load_flow,
        blank
    }
}