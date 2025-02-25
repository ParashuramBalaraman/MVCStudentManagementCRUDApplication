using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCStudentManagementCRUD.Services.String
{
    public class StringService
    {
        public string CapitalizeString(string value)
        {
            if (value == null)
            {
                return value;
            }
            return char.ToUpper(value[0]) + value.Substring(1).ToLower();
        }
    }
}