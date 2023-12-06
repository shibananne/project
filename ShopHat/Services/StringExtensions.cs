namespace ShopHat.Services
{
   
        public static class StringExtensions
        {
            public static string FormatVND(this string numberStr)
            {
                var formattedStr = System.Text.RegularExpressions.Regex.Replace(numberStr, @"(?=(?:\d{3})+$)", " ");
                return formattedStr.Trim() + " VND";
            }
        }
   
}
