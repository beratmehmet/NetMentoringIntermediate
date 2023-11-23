using System.Security.Cryptography;

var result = GeneratePasswordHashUsingSalt("test", new byte[16] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 });
Console.WriteLine(result);

string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
{

    const int SaltSize = 16;
    const int HashSize = 20;
    const int iterate = 10000;

    byte[] hashBytes = new byte[SaltSize + HashSize];
    using (var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate))
    {
        byte[] hash = pbkdf2.GetBytes(HashSize);
        Buffer.BlockCopy(salt, 0, hashBytes, 0, SaltSize);
        Buffer.BlockCopy(hash, 0, hashBytes, SaltSize, HashSize);
    }

    return Convert.ToBase64String(hashBytes);

}
