﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WatchIT.WebAPI.Services.AccountsService.Request;
using WatchIT.WebAPI.Services.AccountsService.Response;

namespace WatchIT.WebAPI.Services.AccountsService
{
    public interface IAccountsService
    {
        void Register(RegisterRequest request);
        AuthenticateResponse Authenticate(string emailOrUsername, string password);
    }
}