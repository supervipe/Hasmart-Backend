using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;


namespace HASmart.Test.Extensions
{
    public static class ActionResultExtensions
    {
        public static T GetValue<T>(this ActionResult<T> action) where T : class {
            return action.Value ?? (T)Convert.ChangeType((action.Result as ObjectResult)?.Value, typeof(T));
        }
    }
}
