﻿using GecolPro.Models.Models;
using Microsoft.AspNetCore.Mvc;
using static GecolPro.Models.Models.MultiRequestUSSD;

namespace GecolPro.WebApi.Interfaces
{
    public interface IUssdProcess
    {
        public Task<ContentResult> GetResponse(string xmlContent, string lang);
        public Task<bool> CheckServiceExist(string? convID = null);
        public Task<bool> CheckDcbExist(string? convID = null);

    }
}
