// perticula - core - FileUtilities.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace core.IO;

public static class FileUtilities
{
	private static readonly string InvalidRegStr;
	public static readonly  char[] InvalidFileNameChars;

	/// <summary>
	///   Initializes static members of the <see cref="FileUtilities" /> class.
	/// </summary>
	static FileUtilities()
	{
		InvalidFileNameChars = GetInvalidFileNameChars().Distinct().ToArray();
		InvalidRegStr        = string.Format(@"([{0}]*\.+$)|([{0}]+)", Regex.Escape(new string(InvalidFileNameChars)));
	}

	/// <summary>
	///   Returns the MD5 has (base64 encoded) for the specified file
	/// </summary>
	/// <param name="fileName">The name of the file to hash</param>
	/// <returns>System.String.</returns>
	public static string GetFileMd5Hash(string fileName)
	{
		using var md5    = MD5.Create();
		using var stream = File.OpenRead(fileName);
		return Convert.ToBase64String(md5.ComputeHash(stream));
	}

	/// <summary>
	///   Gets the file sha256 hash.
	/// </summary>
	/// <param name="fileName">Name of the file.</param>
	/// <returns>System.String.</returns>
	public static string GetFileSha256Hash(string fileName)
	{
		using var sha256 = SHA256.Create();
		using var stream = File.OpenRead(fileName);
		return Convert.ToBase64String(sha256.ComputeHash(stream));
	}

	/// <summary>
	///   Checks if the content of a file on disk matches the newContent
	/// </summary>
	/// <param name="fileName">Name of the file.</param>
	/// <param name="newContent">The new content.</param>
	/// <returns><c>true</c> if [has file content changed] [the specified file name]; otherwise, <c>false</c>.</returns>
	public static bool HasFileContentChanged(string fileName, string newContent)
	{
		if (!File.Exists(fileName)) return true;
		var oldContent = File.ReadAllText(fileName);
		return oldContent != newContent;
	}

	/// <summary>
	///   Finds the relative path between two files.
	/// </summary>
	/// <param name="baseFile">The base file.</param>
	/// <param name="file">The file.</param>
	/// <returns>System.String.</returns>
	public static string MakeRelative(string baseFile, string file)
	{
		var baseUri = new Uri(baseFile, UriKind.RelativeOrAbsolute);
		var fileUri = new Uri(file,     UriKind.RelativeOrAbsolute);

		return Uri.UnescapeDataString(baseUri.MakeRelativeUri(fileUri).ToString());
	}

	/// <summary>
	///   If a file has a read-only attribute, this method will remove it.
	/// </summary>
	/// <param name="fileName">Name of the file.</param>
	public static void RemoveReadonlyFlagFromFile(string fileName)
	{
		var file = new FileInfo(fileName);
		if (file is {Exists: true, IsReadOnly: true})
			file.IsReadOnly = false;
	}

	/// <summary>
	///   If a file has the read-only attribute, this method will remove it.
	/// </summary>
	/// <param name="file">The file.</param>
	public static void RemoveReadonlyFlagFromFile(FileInfo file) => RemoveReadonlyFlagFromFile(file.FullName);

	/// <summary>
	///   Removes the temporary files.
	/// </summary>
	/// <param name="tempFile">The temporary file.</param>
	public static void RemoveTempFiles(string tempFile)
	{
		var fi   = new FileInfo(tempFile);
		var dir  = fi.Directory;
		var path = fi.Name;

		if (dir == null) return;
		var allFiles = dir.GetFiles(path + "*", SearchOption.TopDirectoryOnly);
		foreach (var file in allFiles) File.Delete(file.FullName);
	}

	/// <summary>
	///   Sanitizes the name of a file by converting invalid characters to an underscore.
	/// </summary>
	/// <param name="fileName">Name of the file.</param>
	/// <returns>System.String.</returns>
	/// <exception cref="System.ArgumentNullException">fileName</exception>
	public static string SanitizeFileName(string fileName)
	{
		if (fileName == null) throw new ArgumentNullException(nameof(fileName));
		return Regex.Replace(fileName, InvalidRegStr, "_");
	}

	/// <summary>
	///   Gets the characters we declare as invalid for filenames.
	///   includes system values, as well as custom rules.
	/// </summary>
	/// <returns>IEnumerable&lt;System.Char&gt;.</returns>
	private static IEnumerable<char> GetInvalidFileNameChars()
	{
		foreach (var c in Path.GetInvalidFileNameChars()) yield return c;

		yield return '\'';
		yield return '"';
		yield return '\\';
		yield return '/';
		yield return ':';
		yield return '*';
		yield return '?';
		yield return '<';
		yield return '>';
		yield return '|';
	}
}
