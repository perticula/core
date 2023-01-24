// perticula - core - SerializableBase.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using Newtonsoft.Json;

namespace core.Serialization;

/// <summary>
///   Class SerializableBase.
/// </summary>
/// <typeparam name="TParent">The type of the t parent.</typeparam>
public abstract class SerializableBase<TParent> where TParent : SerializableBase<TParent>
{
  /// <summary>
  ///   The name of this file from which this object was loaded or to which
  ///   this object was last Saves.
  /// </summary>
  /// <value>The name of the file.</value>
  [JsonIgnore]
  public string? FileName { get; set; }

  /// <summary>
  ///   Loads this object from a Json string
  /// </summary>
  /// <param name="objValue">The string containing the json</param>
  /// <returns>TParent.</returns>
  /// <exception cref="System.ArgumentNullException">objValue</exception>
  /// <exception cref="System.Exception">Unable to deserialize string</exception>
  public static TParent FromJson(string objValue)
  {
    if (string.IsNullOrEmpty(objValue)) throw new ArgumentNullException(nameof(objValue));
    using var sz   = new StringReader(objValue);
    var       json = new JsonSerializer();
    try
    {
      var result = (TParent) json.Deserialize(sz, typeof(TParent))!;
      if (result == null) throw new Exception("Unable to deserialize string");
      result.FileName = "";
      return result;
    }
    catch (Exception ex)
    {
      throw new Exception("Unable to deserialize string", ex);
    }
  }

  /// <summary>
  ///   Loads this object from a string containing Json
  /// </summary>
  /// <param name="stream">The stream containing the json</param>
  /// <returns>TParent.</returns>
  /// <exception cref="System.Exception">Unable to deserialize stream</exception>
  public static TParent FromJson(Stream stream)
  {
    using var file = new StreamReader(stream);
    var       json = new JsonSerializer();
    try
    {
      var result = (TParent) json.Deserialize(file, typeof(TParent))!;
      if (result == null) throw new Exception("Unable to deserialize stream");
      result.FileName = "";
      return result;
    }
    catch (Exception ex)
    {
      throw new Exception("Unable to deserialize stream", ex);
    }
  }

  /// <summary>
  ///   Loads this object from a Json string
  /// </summary>
  /// <param name="objValue">The string containing the json</param>
  /// <returns>IEnumerable&lt;TParent&gt;.</returns>
  /// <exception cref="System.ArgumentNullException">objValue</exception>
  /// <exception cref="System.Exception">Unable to deserialize file</exception>
  public static IEnumerable<TParent> FromJsonList(string objValue)
  {
    if (string.IsNullOrEmpty(objValue)) throw new ArgumentNullException(nameof(objValue));

    try
    {
      var result = JsonConvert.DeserializeObject<List<TParent>>(objValue);
      if (result != null) return result;
      throw new Exception("Unable to deserialize file");
    }
    catch (Exception ex)
    {
      throw new Exception("Unable to deserialize file", ex);
    }
  }

  /// <summary>
  ///   Loads this object from a JSON file
  /// </summary>
  /// <param name="fileName">The path and name of the JSON file</param>
  /// <returns>TParent.</returns>
  /// <exception cref="System.IO.FileNotFoundException">File not found</exception>
  /// <exception cref="System.Exception">Unable to deserialize file</exception>
  public static TParent LoadJson(string fileName)
  {
    using var file = File.OpenText(fileName);
    if (file == null) throw new FileNotFoundException("File not found", fileName);

    try
    {
      var json   = new JsonSerializer();
      var result = (TParent) json.Deserialize(file, typeof(TParent))!;
      if (result == null) throw new Exception("Unable to deserialize file");
      result.FileName = fileName;
      return result;
    }
    catch
    {
      throw new Exception("Unable to deserialize file");
    }
  }

  /// <summary>
  ///   Saves this object to a JSON file
  /// </summary>
  /// <param name="fileName">The path and name of the JSON file</param>
  public void SaveJson(string fileName)
  {
    using var file = File.CreateText(fileName);
    var       json = new JsonSerializer();
    json.Serialize(file, (TParent) this);
    FileName = fileName;
  }

  /// <summary>
  ///   Saves this object to a Json string
  /// </summary>
  /// <returns>System.String.</returns>
  public string ToJson()
  {
    using var sz   = new StringWriter();
    var       json = new JsonSerializer();
    json.Serialize(sz, (TParent) this);
    FileName = "";
    return sz.ToString();
  }
}
