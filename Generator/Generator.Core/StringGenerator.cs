using Generator.Abstractions;

namespace Generator.Core
{
    /// <summary>
    /// Generates random strings of a certain format
    /// </summary>
    public class StringGenerator : IStringGenerator
    {
        private readonly Random _random = new Random();
        private readonly DateTime _startDate = new DateTime(2018, 8, 5);
        private readonly string _latinSymbols = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private readonly string _russianSymbols = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя";

        /// <summary>
        /// Generating strings
        /// </summary>
        /// <param name="stringSpan">Span for string generation. Max lenght = 61 char because of our string standart</param>
        /// <returns></returns>
        public int GenerateString(ref Span<char> stringSpan)
        {
            int spanLength = 0;
            DateTime randomDate = _startDate.AddDays(_random.Next((DateTime.Today - _startDate).Days));
            randomDate.TryFormat(stringSpan, out spanLength, "dd.MM.yyyy".AsSpan());

            stringSpan[spanLength++] = '|';
            stringSpan[spanLength++] = '|';

            for (int i = 0; i < 10; i++)
            {
                stringSpan[spanLength++] = _latinSymbols[_random.Next(_latinSymbols.Length)];
            }

            stringSpan[spanLength++] = '|';
            stringSpan[spanLength++] = '|';

            for (int i = 0; i < 10; i++)
            {
                stringSpan[spanLength++] = _russianSymbols[_random.Next(_russianSymbols.Length)];
            }

            stringSpan[spanLength++] = '|';
            stringSpan[spanLength++] = '|';
            //span for generated integer(the generated int can be from 1 to 100000000, so our span takes 9 chars) 
            Span<char> intSpan = stackalloc char[9];
            int intSpanLength = 0;
            int randomInt = _random.Next(1 / 2, 100000000 / 2) * 2;
            randomInt.TryFormat(intSpan, out intSpanLength);
            intSpan.CopyTo(stringSpan.Slice(spanLength));
            spanLength += intSpanLength;

            stringSpan[spanLength++] = '|';
            stringSpan[spanLength++] = '|';

            //span for generated double(the generated double can have up to 2 digits in the integer part and up to 8 digits in the fractional part + one char for coma, so our span takes 11 chars) 
            Span<char> doubleSpan = stackalloc char[11];
            int doubleSpanLength = 0;
            double randomDouble = _random.NextDouble() * 19.0 + 1.0;
            randomDouble.TryFormat(doubleSpan, out doubleSpanLength, "F8".AsSpan());
            doubleSpan.CopyTo(stringSpan.Slice(spanLength));
            spanLength += doubleSpanLength;

            stringSpan[spanLength++] = '|';
            stringSpan[spanLength++] = '|';

            return spanLength;
        }
    }
}