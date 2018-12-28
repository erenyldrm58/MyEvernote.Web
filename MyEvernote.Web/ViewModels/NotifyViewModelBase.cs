using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.Web.ViewModels
{
    public class NotifyViewModelBase<T>
    {
        public List<T> Items { get; set; }
        public string Header { get; set; }
        public string Title { get; set; }
        public bool IsRedirect { get; set; }
        public string RedirectUrl { get; set; }
        public int RedirectTimeout { get; set; }

        public NotifyViewModelBase()
        {
            Header = "Yönlendiriliyorsunuz..";
            Title = "Geçersiz İşlem";
            IsRedirect = true;
            RedirectUrl = "/Home/Index/";
            RedirectTimeout = 3000;
            Items = new List<T>();
        }
    }
}