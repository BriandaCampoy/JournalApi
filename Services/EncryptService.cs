namespace journalapi.Services
{
    public class EncryptService
    {
        public string Encrypt(string code)
        {
            byte[] encData_byte = new byte[code.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(code);
            return Convert.ToBase64String(encData_byte);
        }

        public string Decrypt (string code)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(code);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            return new String(decoded_char);
        }

    }
}
