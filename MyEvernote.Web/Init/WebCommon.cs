using MyEvernote.Common;
using MyEvernote.Entities;
using MyEvernote.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.Web.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            EvernoteUser user = CurrentSession.User;
            if (user != null)
                return user.Username;
            else
                return "system";
        }
    }
}