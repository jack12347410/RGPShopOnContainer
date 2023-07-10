using AuthApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApi.Data
{
    public class IdentityDbInit
    {
        /// <summary>
        /// 確認是否有測試的帳號，沒有的話則新增
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <returns></returns>
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            context.Database.Migrate();
            if (context.Users.Any(x => x.UserName.Equals("test@mail.com"))) return;

            string user = "test@mail.com";
            string password = "p@a#$ss%word";

            await userManager.CreateAsync(new ApplicationUser { UserName = user, EmailConfirmed = true }, password);
        }
    }
}
