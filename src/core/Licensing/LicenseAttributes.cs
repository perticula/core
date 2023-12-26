// perticula - core - LicenseAttributes.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Xml.Linq;

namespace core.Licensing;

/// <summary>
///   Class LicenseAttributes.
/// </summary>
public class LicenseAttributes
{
	/// <summary>
	///   The child name
	/// </summary>
	protected readonly XName ChildName;

	/// <summary>
	///   The XML data
	/// </summary>
	protected readonly XElement XmlData;

	/// <summary>
	///   Initializes a new instance of the <see cref="LicenseAttributes" /> class.
	/// </summary>
	/// <param name="xmlData">The XML data.</param>
	/// <param name="childName">Name of the child.</param>
	internal LicenseAttributes(XElement xmlData, XName childName)
	{
		XmlData   = xmlData;
		ChildName = childName;
	}

	/// <summary>
	///   Adds the specified key.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <param name="value">The value.</param>
	public virtual void Add(string key, string value) => SetChildTag(key, value);

	/// <summary>
	///   Adds all.
	/// </summary>
	/// <param name="features">The features.</param>
	public virtual void AddAll(IDictionary<string, string> features)
	{
		foreach (var feature in features)
			Add(feature.Key, feature.Value);
	}

	/// <summary>
	///   Removes the specified key.
	/// </summary>
	/// <param name="key">The key.</param>
	public virtual void Remove(string key)
	{
		var element = XmlData.Elements(ChildName)
			.FirstOrDefault(e => e.Attribute("name") != null && e.Attribute("name")?.Value == key);
		element?.Remove();
	}

	/// <summary>
	///   Removes all.
	/// </summary>
	public virtual void RemoveAll() => XmlData.RemoveAll();

	/// <summary>
	///   Gets the specified key.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <returns>System.Nullable&lt;System.String&gt;.</returns>
	public virtual string? Get(string key) => GetChildTag(key);

	/// <summary>
	///   Gets all.
	/// </summary>
	/// <returns>IDictionary&lt;System.String, System.String&gt;.</returns>
	public virtual IDictionary<string, string> GetAll() => XmlData.Elements(ChildName)
		.ToDictionary(e => e.Attribute("name")?.Value ?? "", e => e.Value);

	/// <summary>
	///   Determines whether this instance contains the object.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <returns><c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.</returns>
	public virtual bool Contains(string key) => XmlData.Elements(ChildName)
		.Any(e => e.Attribute("name") != null && e.Attribute("name")?.Value == key);

	/// <summary>
	///   Determines whether the specified keys contains all.
	/// </summary>
	/// <param name="keys">The keys.</param>
	/// <returns><c>true</c> if the specified keys contains all; otherwise, <c>false</c>.</returns>
	public virtual bool ContainsAll(string[] keys) => XmlData.Elements(ChildName)
		.All(e => e.Attribute("name") != null && keys.Contains(e.Attribute("name")?.Value));

	/// <summary>
	///   Sets the tag.
	/// </summary>
	/// <param name="name">The name.</param>
	/// <param name="value">The value.</param>
	protected virtual void SetTag(string name, string value)
	{
		var element = XmlData.Element(name);

		if (element == null)
		{
			element = new XElement(name);
			XmlData.Add(element);
		}

		element.Value = value;
	}

	/// <summary>
	///   Sets the child tag.
	/// </summary>
	/// <param name="name">The name.</param>
	/// <param name="value">The value.</param>
	protected virtual void SetChildTag(string name, string value)
	{
		var element = XmlData.Elements(ChildName)
			.FirstOrDefault(e => e.Attribute("name") != null && e.Attribute("name")?.Value == name);

		if (element == null)
		{
			element = new XElement(ChildName);
			element.Add(new XAttribute("name", name));
			XmlData.Add(element);
		}

		element.Value = value;
	}

	/// <summary>
	///   Gets the tag.
	/// </summary>
	/// <param name="name">The name.</param>
	/// <returns>System.Nullable&lt;System.String&gt;.</returns>
	protected virtual string? GetTag(string name) => XmlData.Element(name)?.Value;

	/// <summary>
	///   Gets the child tag.
	/// </summary>
	/// <param name="name">The name.</param>
	/// <returns>System.Nullable&lt;System.String&gt;.</returns>
	protected virtual string? GetChildTag(string name) => XmlData.Elements(ChildName)
		.FirstOrDefault(e => e.Attribute("name") != null && e.Attribute("name")?.Value == name)?.Value;
}
