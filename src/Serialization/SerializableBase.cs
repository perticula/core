
using Newtonsoft.Json;

namespace core;


/// <summary>
///     Class SerializableBase.
/// </summary>
/// <typeparam name="TParent">The type of the t parent.</typeparam>
public abstract class SerializableBase<TParent> where TParent : SerializableBase<TParent>
{
  /// <summary>
  ///     The name of this file from which this object was loaded or to which
  ///     this object was last Saves.
  /// </summary>
  /// <value>The name of the file.</value>
  [JsonIgnore]
  public string? FileName { get; set; }

  /// <summary>
  ///     Loads this object from a Json string
  /// </summary>
  /// <param name="objValue">The string containing the json</param>
  /// <returns>TParent.</returns>
  public static TParent FromJson(string objValue)
  {
    if (string.IsNullOrEmpty(objValue)) throw new ArgumentNullException(nameof(objValue));
    using var sz = new StringReader(objValue);
    var json = new JsonSerializer();
    try
    {
      var result = (TParent)json.Deserialize(sz, typeof(TParent));
      if (result != null)
      {
        result.FileName = "";
        return result;
      }
      throw new Exception("Unable to deserialize string");
    }
    catch (Exception ex)
    {
      throw new Exception("Unable to deserialize string", ex);
    }
  }

  /// <summary>
  ///     Loads this object from a string containing Json
  /// </summary>
  /// <param name="stream">The stream containing the json</param>
  /// <returns>TParent.</returns>
  public static TParent FromJson(Stream stream)
  {
    using var file = new StreamReader(stream);
    var json = new JsonSerializer();
    try
    {
      var result = (TParent)json.Deserialize(file, typeof(TParent));
      if (result != null)
      {
        result.FileName = "";
        return result;
      }
      throw new Exception("Unable to deserialize stream");
    }
    catch (Exception ex)
    {
      throw new Exception("Unable to deserialize stream", ex);
    }
  }

  /// <summary>
  ///     Loads this object from a Json string
  /// </summary>
  /// <param name="objValue">The string containing the json</param>
  /// <returns>IEnumerable&lt;TParent&gt;.</returns>
  public static IEnumerable<TParent> FromJsonList(string objValue)
  {
    if (string.IsNullOrEmpty(objValue)) throw new ArgumentNullException(nameof(objValue));

    try
    {
      var result = JsonConvert.DeserializeObject<List<TParent>>(objValue);
      if (result != null)
        return result;
      throw new Exception("Unable to deserialize file");
    }
    catch (Exception ex)
    {
      throw new Exception("Unable to deserialize file", ex);
    }
  }

  /// <summary>
  ///     Loads this object from a JSON file
  /// </summary>
  /// <param name="fileName">The path and name of the JSON file</param>
  /// <returns>TParent.</returns>
  public static TParent LoadJson(string fileName)
  {
    using var file = File.OpenText(fileName);
    if (file == null)
    {
      throw new FileNotFoundException("File not found", fileName);
    }

    try
    {
      var json = new JsonSerializer();
      var result = (TParent)json.Deserialize(file, typeof(TParent));
      if (result != null)
      {
        result.FileName = fileName;
        return result;
      }
      throw new Exception("Unable to deserialize file");
    }
    catch
    {
      throw new Exception("Unable to deserialize file");
    }
  }

  /// <summary>
  ///     Saves this object to a JSON file
  /// </summary>
  /// <param name="fileName">The path and name of the JSON file</param>
  public void SaveJson(string fileName)
  {
    using var file = File.CreateText(fileName);
    var json = new JsonSerializer();
    json.Serialize(file, (TParent)this);
    FileName = fileName;
  }

  /// <summary>
  ///     Saves this object to a Json string
  /// </summary>
  /// <returns>System.String.</returns>
  public string ToJson()
  {
    using var sz = new StringWriter();
    var json = new JsonSerializer();
    json.Serialize(sz, (TParent)this);
    FileName = "";
    return sz.ToString();
  }
}
