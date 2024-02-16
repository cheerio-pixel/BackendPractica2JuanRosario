
namespace backend.Utils
{
    internal class Util
    {
        // Source https://en.wikipedia.org/wiki/Luhn_algorithm#C#
        public static bool IsValidLuhn(in int[] digits)
        {
            int check_digit = 0;
            for (int i = digits.Length - 2; i >= 0; --i)
            {
                check_digit += ((i & 1) is 0) switch
                {
                    true => digits[i] > 4 ? (digits[i] * 2) - 9 : digits[i] * 2,
                    false => digits[i]
                };
            }

            return 10 - (check_digit % 10) == digits.Last();
        }

    }
}