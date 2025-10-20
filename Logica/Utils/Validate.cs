using System.Text.RegularExpressions;

namespace Logica.Utils
{
    public static class Validate
    {
        public static bool NIF(string nif)
        {
            if (string.IsNullOrWhiteSpace(nif) || nif.Length != 9)
                return false;

            string numeros = nif.Substring(0, 8);
            char letra = char.ToUpper(nif[8]);

            if (!int.TryParse(numeros, out int numero))
                return false;

            string letras = "TRWAGMYFPDXBNJZSQVHLCKE";
            char letraCorrecta = letras[numero % 23];

            return letra == letraCorrecta;
        }

        // Validación de IBAN español
        public static bool IBAN(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
                return false;

            iban = iban.Replace(" ", "").ToUpper();

            if (!Regex.IsMatch(iban, @"^ES\d{22}$"))
                return false;

            // Mover los 4 primeros caracteres al final
            string reformulado = iban.Substring(4) + iban.Substring(0, 4);

            // Convertir letras a números (A=10, B=11, ..., Z=35)
            string numerico = "";
            foreach (char c in reformulado)
            {
                if (char.IsLetter(c))
                    numerico += (c - 'A' + 10).ToString();
                else
                    numerico += c;
            }

            // Validar con módulo 97
            return Modulo97(numerico) == 1;
        }

        private static int Modulo97(string input)
        {
            string fragmento = "";
            foreach (char c in input)
            {
                fragmento += c;
                if (fragmento.Length >= 9)
                {
                    int num = int.Parse(fragmento);
                    fragmento = (num % 97).ToString();
                }
            }
            return int.Parse(fragmento) % 97;
        }

        public static bool Email(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string patron = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(patron);

            if (!regex.IsMatch(email))
                return false;

            return true;
        }

        public static bool Telf(string telf)
        {
            if (string.IsNullOrWhiteSpace(telf))
                return false;

            string patron = @"^(?:\+34|0034|34)?[6789]\d{8}$";
            Regex regex = new Regex(patron);

            if (!regex.IsMatch(telf))
                return false;

            return true;
        }
    }
}