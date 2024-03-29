using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.CustomerPortal.Application.Utilities
{
    public static class HashUtility
    {
		public static string ComputeSha256Hash(string inputString)
		{
			// Create a SHA256   
			using (SHA256 sha256Hash = SHA256.Create())
			{
				// ComputeHash - returns byte array  
				byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(inputString));

				// Convert byte array to a string   
				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < bytes.Length; i++)
				{
					builder.Append(bytes[i].ToString("x2"));
				}
				return builder.ToString();
			}
		}

		public static bool ValidateSha256Hash(string hash, string inputString)
		{
			var newHash = ComputeSha256Hash(inputString);
			return hash == newHash;
		}

		public static string ComputeMD5Hash(string inputString)
		{
			// Use input string to calculate MD5 hash
			using (MD5 md5 = MD5.Create())
			{
				byte[] inputBytes = Encoding.ASCII.GetBytes(inputString);
				byte[] hashBytes = md5.ComputeHash(inputBytes);

				// Convert the byte array to hexadecimal string
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < hashBytes.Length; i++)
				{
					sb.Append(hashBytes[i].ToString("X2"));
				}
				return sb.ToString();
			}
		}

		public static bool ValidateMD5Hash(string hash, string inputString)
		{
			var newHash = ComputeMD5Hash(inputString);
			return hash == newHash;
		}
	}
}
