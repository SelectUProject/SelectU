using System.Text;

namespace SelectU.Core.Helpers
{
    public static class PasswordHelper
    {
        static Random random = new Random();

        public static string GenerateRandomPassword(int length, int symbolCount)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            const string symbols = "!@#$%^&*()_+";
            StringBuilder password = new StringBuilder();

            for (int i = 0; i < length - symbolCount; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }

            for (int i = 0; i < symbolCount; i++)
            {
                password.Append(symbols[random.Next(symbols.Length)]);
            }

            // Shuffle the characters to mix symbols in random positions
            for (int i = 0; i < password.Length; i++)
            {
                int randomIndex = random.Next(password.Length);
                char temp = password[i];
                password[i] = password[randomIndex];
                password[randomIndex] = temp;
            }

            return password.ToString();
        }
    }
}
