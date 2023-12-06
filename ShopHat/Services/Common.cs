﻿using Microsoft.EntityFrameworkCore;
using System.Net;

using Microsoft.AspNetCore.Identity;

using System.Drawing.Imaging;
using Path = System.IO.Path;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Collections;
using System.Globalization;
using System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShopHat.Services;
using ShopHat.Data;
using System.Security.Cryptography;
using System.Text;
using ShopHat.Models;


namespace ShopHat.Services
{
    public class Common : ICommon
    {
        private readonly IWebHostEnvironment _iHostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public Common(IWebHostEnvironment iHostingEnvironment, ApplicationDbContext context, IConfiguration configuration)
        {
            _iHostingEnvironment = iHostingEnvironment;
            _context = context;
            _configuration = configuration;
        }

        public Common()
        {
        }

        public string GetContentRootPath()
        {
            return _iHostingEnvironment.ContentRootPath;
        }
        public string GetFolderUploadPath()
        {
            return Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/Upload");
        }

        public async Task<string> UploadedFile(IFormFile ProfilePicture)
        {
            string ProfilePictureFileName = null;

            if (ProfilePicture != null)
            {
                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/Upload");

                if (ProfilePicture.FileName == null)
                    ProfilePictureFileName = Guid.NewGuid().ToString() + "_" + "blank-person.png";
                else
                    ProfilePictureFileName = Guid.NewGuid().ToString() + "_" + ProfilePicture.FileName;
                string filePath = Path.Combine(uploadsFolder, ProfilePictureFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ProfilePicture.CopyTo(fileStream);
                }
            }
            return ProfilePictureFileName;
        }
        public string GetSHA256(string str)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
		public string GetMD5(string str)
		{
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] fromData = Encoding.UTF8.GetBytes(str);
			byte[] targetData = md5.ComputeHash(fromData);
			string byte2String = null;

			for (int i = 0; i < targetData.Length; i++)
			{
				byte2String += targetData[i].ToString("x2");

			}
			return byte2String;
		}
		//     public void SendEmail(DataUser request)
		//     {

		//         var message = new MimeMessage();
		//         message.From.Add(new MailboxAddress("", _configuration.GetSection("EmailUserName").Value));
		//         message.To.Add(MailboxAddress.Parse(request.UserName));
		//         message.Subject = "Khôi phục mật khẩu";

		//         var builder = new BodyBuilder();
		////builder.HtmlBody = $"Chào {request.UserName},{Environment.NewLine}{Environment.NewLine}Bạn đã yêu cầu khôi phục mật khẩu tại hệ thống của chúng tôi.{Environment.NewLine}Mã xác thực của bạn là {request.TokenPassword}.{Environment.NewLine}Mã xác thực này sẽ hết hạn trong vòng 30 phút. Vui lòng đăng nhập và thay đổi mật khẩu của bạn trong khoảng thời gian này.{Environment.NewLine}{Environment.NewLine}Trân trọng,{Environment.NewLine}Hệ thống quản lý.";

		//message.Body = builder.ToMessageBody();

		//         using (var client = new SmtpClient())
		//         {
		//             client.Connect(_configuration.GetSection("EmailHost").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
		//             client.Authenticate(_configuration.GetSection("EmailUserName").Value, _configuration.GetSection("EmailPassword").Value);
		//             client.Send(message);
		//             client.Disconnect(true);
		//         }
		//     }

		//     public string GenerateToken()
		//     {
		//         // Tạo mã ngẫu nhiên bằng cách sử dụng các ký tự khác nhau.
		//         const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		//         var random = new Random();
		//         var token = new string(
		//             Enumerable.Repeat(chars, 20)
		//             .Select(s => s[random.Next(s.Length)])
		//             .ToArray());

		//         return token;
		//     }
	}

       
}