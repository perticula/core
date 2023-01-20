using System.Text;
using System.Security.Cryptography;
namespace core;

/// <summary>
/// Class SimpleEncrypt.
/// </summary>
public class SimpleEncrypt
{
  /// <summary>
  /// The key
  /// </summary>
  private static readonly byte[] Key = { 123, 217, 19, 11, 15, 26, 85, 16, 114, 184, 27, 162, 4, 112, 222, 209, 0, 24, 175, 167, 173, 53, 182, 29, 24, 26, 17, 218, 247, 236, 53, 209 };

  /// <summary>
  /// The vector
  /// </summary>
  private static readonly byte[] Vector = { 24, 64, 191, 123, 46, 3, 17, 119, 67, 121, 123, 112, 79, 32, 114, 156 };

  /// <summary>
  /// The decryptor
  /// </summary>
  private readonly ICryptoTransform _decryptor;

  /// <summary>
  /// The encoder
  /// </summary>
  private readonly UTF8Encoding _encoder;

  /// <summary>
  /// The encryptor
  /// </summary>
  private readonly ICryptoTransform _encryptor;

  /// <summary>
  /// Initializes a new instance of the <see cref="SimpleEncrypt"/> class.
  /// </summary>
  public SimpleEncrypt()
  {
    var rm = Aes.Create("AesManaged");
    if (rm == null) { throw new Exception("Could not create AesManaged instance"); }
    _encryptor = rm.CreateEncryptor(Key, Vector);
    _decryptor = rm.CreateDecryptor(Key, Vector);
    _encoder = new UTF8Encoding();
  }

  /// <summary>
  /// Decrypts the specified encrypted.
  /// </summary>
  /// <param name="encrypted">The encrypted.</param>
  /// <returns>System.String.</returns>
  public string Decrypt(string encrypted) => string.IsNullOrEmpty(encrypted) ? string.Empty : _encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));

  /// <summary>
  /// Decrypts the specified buffer.
  /// </summary>
  /// <param name="buffer">The buffer.</param>
  /// <returns>System.Byte[].</returns>
  /// <exception cref="ArgumentNullException">buffer</exception>
  public byte[] Decrypt(byte[] buffer)
  {
    if (buffer == null) throw new ArgumentNullException(nameof(buffer));
    return Transform(buffer, _decryptor);
  }

  /// <summary>
  /// Encrypts the specified unencrypted.
  /// </summary>
  /// <param name="unencrypted">The unencrypted.</param>
  /// <returns>System.String.</returns>
  public string Encrypt(string unencrypted) => string.IsNullOrEmpty(unencrypted) ? string.Empty : Convert.ToBase64String(Encrypt(_encoder.GetBytes(unencrypted)));

  /// <summary>
  /// Encrypts the specified buffer.
  /// </summary>
  /// <param name="buffer">The buffer.</param>
  /// <returns>System.Byte[].</returns>
  /// <exception cref="ArgumentNullException">buffer</exception>
  public byte[] Encrypt(byte[] buffer)
  {
    if (buffer == null) throw new ArgumentNullException(nameof(buffer));
    return Transform(buffer, _encryptor);
  }

  /// <summary>
  /// Transforms the specified buffer.
  /// </summary>
  /// <param name="buffer">The buffer.</param>
  /// <param name="transform">The transform.</param>
  /// <returns>System.Byte[].</returns>
  /// <exception cref="ArgumentNullException">
  /// buffer
  /// or
  /// transform
  /// </exception>
  protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
  {
    if (buffer == null) throw new ArgumentNullException(nameof(buffer));
    if (transform == null) throw new ArgumentNullException(nameof(transform));

    var stream = new MemoryStream();
    using (var cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
      cs.Write(buffer, 0, buffer.Length);
    return stream.ToArray();
  }
}

public static class __SimpleEncryptExt
{
  /// <summary>
  ///     Decrypts a value.
  /// </summary>
  /// <param name="value">The value.</param>
  /// <returns>System.String.</returns>
  public static string DecryptValue(this string value) => new SimpleEncrypt().Decrypt(value);

  /// <summary>
  ///     Encrypts a value.
  /// </summary>
  /// <param name="value">The value.</param>
  /// <returns>System.String.</returns>
  public static string EncryptValue(this string value) => new SimpleEncrypt().Encrypt(value);
}
