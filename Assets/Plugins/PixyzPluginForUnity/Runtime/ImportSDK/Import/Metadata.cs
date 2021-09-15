using Pixyz.OptimizeSDK.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Pixyz.ImportSDK {

    /// <summary>
    /// When attached to a GameObject (as a Component), the Metadata will store and display all metadata properties.<br/>
    /// Those properties can be automatically imported from the Importer (with the correct settings).
    /// </summary>
    /// <example>
    /// <code>
    /// Metadata metadata = gameObject.GetComponent&lt;Metadata&gt;()
    /// string partNumber = metadata.getProperty("Part Number");
    /// </code>
    /// </example>
    public sealed class Metadata : MonoBehaviour {

        public string type = "Metadata";

        public bool isInstance { get; internal set; }

        private Properties properties {
            get {
                if (_properties == null) {
                    _properties = new Properties();
                }
                if (_properties.names == null) {
                    _properties.names = new string[0];
                }
                if (_properties.values == null) {
                    _properties.values = new string[0];
                }
                return _properties;
            }
        }

        [SerializeField]
        private Properties _properties;

        /// <summary>
        /// Sets the properties from Native Interface.
        /// </summary>
        /// <param name="properties">Native properties</param>
        public void setProperties(Properties properties) {
            _properties = properties;
        }

        /// <summary>
        /// Returns native properties
        /// </summary>
        /// <returns>Native properties</returns>
        public Properties getPropertiesNative() {
            return properties;
        }

        /// <summary>
        /// Returns a property value from its name.
        /// This will produce an exception if no property exists with the given name.
        /// This is case sensitive.
        /// </summary>
        /// <param name="propertyName">Name of the property to look for</param>
        /// <returns>The property value (string)</returns>
        public string getProperty(string propertyName) {
            for (int i = 0; i < properties.names.Length; i++) {
                if (_properties.names[i] == propertyName) {
                    return _properties.values[i];
                }
            }
            throw new System.Exception("Metadata on object " + gameObject.name + " doesn't contains property named '" + propertyName + "'");
        }

        /// <summary>
        /// Returns true if the metadata contains a property with the given name.
        /// This is case sensitive.
        /// </summary>
        /// <param name="propertyName">Name of the property to look for</param>
        /// <returns>True if the property exists</returns>
        public bool containsProperty(string propertyName) {
            for (int i = 0; i < properties.names.Length; i++) {
                if (_properties.names[i] == propertyName) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the properties as a Dictionary.
        /// </summary>
        /// <returns>A Dictionary with all properties</returns>
        public Dictionary<string, string> getProperties() {
            Dictionary<string, string> propsDic = new Dictionary<string, string>();
            for (int i = 0; i < properties.names.Length; i++) {
                string propName = _properties.names[i];
                while (propsDic.ContainsKey(propName)) {
                    propName += '_';
                }
                propsDic.Add(propName, _properties.values[i]);
            }
            return propsDic;
        }

        /// <summary>
        /// Removes a property.
        /// Returns true if the operation was successful.
        /// This is case sensitive.
        /// </summary>
        /// <param name="propertyName">Name of the property to look for</param>
        /// <returns>True if the property was removed. False if the property doesn't exists.</returns>
        public bool removeProperty(string propertyName) {
            for (int i = 0; i < properties.names.Length; i++) {
                if (_properties.names[i] == propertyName) {
                    CollectionExtensions.RemoveAt(ref _properties.names, i);
                    CollectionExtensions.RemoveAt(ref _properties.values, i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds or set an existing property value.
        /// This is case sensitive.
        /// </summary>
        /// <param name="propertyName">Name of the property to look for</param>
        /// <param name="propertyValue">Value of the property to add or set</param>
        public void addOrSetProperty(string propertyName, string propertyValue) {
            for (int i = 0; i < properties.names.Length; i++) {
                if (_properties.names[i] == propertyName) {
                    _properties.values[i] = propertyValue;
                    return;
                }
            }
            CollectionExtensions.Append(ref _properties.names, propertyName);
            CollectionExtensions.Append(ref _properties.values, propertyValue);
        }
    }
}