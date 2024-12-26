using GecolPro.BusinessRules.Interfaces;
using Newtonsoft.Json;

namespace GecolPro.BusinessRules.BusinessRules
{
    public class BlackListFun : IBlackListFun
    {
        private readonly string jsonFilePath;

        public BlackListFun()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            jsonFilePath = Path.Combine(baseDirectory, "BlackListMsisdn.json");
        }

        public async Task SyncBlackList(List<string> blackMsisdnList)
        {
            try
            {

                if (blackMsisdnList == null || blackMsisdnList.Count == 0)
                {
                    //return BadRequest("Invalid phone numbers list.");
                }

                List<string> blackList = new List<string>();

                if (System.IO.File.Exists(jsonFilePath))
                {
                    var json = await System.IO.File.ReadAllTextAsync(jsonFilePath);
                    blackList = JsonConvert.DeserializeObject<List<string>>(json) ?? new List<string>();
                }

                foreach (var newNumber in blackMsisdnList)
                {
                    if (!blackList.Exists(number => newNumber.StartsWith(number)))
                    {
                        blackList.Add(newNumber);
                    }
                }

                var updatedJson = JsonConvert.SerializeObject(blackList, Formatting.Indented);
                await System.IO.File.WriteAllTextAsync(jsonFilePath, updatedJson);
            }
            catch (Exception ex)
            {
                string message = ex.Message;

            }
        }

        public async Task<List<string>> GetBlackList()
        {

            if (!System.IO.File.Exists(jsonFilePath))
            {
                return new List<string>();
            }

            var json = await System.IO.File.ReadAllTextAsync(jsonFilePath);
            var blackList = JsonConvert.DeserializeObject<List<string>>(json) ?? new List<string>();

            return blackList;
        }


        public async Task SyncDeleteBlackList(string phoneNumber)
        {
            try
            {            
                string? res;

                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    res = "Invalid phone number.";
                }

                if (!System.IO.File.Exists(jsonFilePath))
                {
                    res = "Blacklist not found.";
                }

                var json = await System.IO.File.ReadAllTextAsync(jsonFilePath);
                var blacklist = JsonConvert.DeserializeObject<List<string>>(json) ?? new List<string>();

                if (!blacklist.Remove(phoneNumber))
                {

                    res = "Phone number not found in the blacklist.";
                }

                var updatedJson = JsonConvert.SerializeObject(blacklist, Formatting.Indented);
                await System.IO.File.WriteAllTextAsync(jsonFilePath, updatedJson);
            }
            catch (Exception ex)
            {
                string message = ex.Message;

            }
        }


    }
}
