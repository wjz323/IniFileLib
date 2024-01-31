using System;
using System.Collections.Generic;
using System.IO;

namespace IniFileLib;

/// <summary>
/// 提供读写 INI 文件的静态方法
/// </summary>
public static class IniFile
{
    /// <summary>
    /// 从指定路径读取 INI 文件并返回其内容
    /// </summary>
    /// <param name="filePath">INI 文件路径</param>
    /// <returns>包含 INI 文件内容的字典</returns>
    public static Dictionary<string, Dictionary<string, string>> Read(string filePath)
    {
        var iniData = new Dictionary<string, Dictionary<string, string>>();

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("INI file not found.", filePath);
        }

        string currentSection = null;
        Dictionary<string, string> currentSectionData = null;

        foreach (string line in File.ReadLines(filePath))
        {
            string trimmedLine = line.Trim();

            if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
            {
                currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2);
                currentSectionData = new Dictionary<string, string>();
                iniData[currentSection] = currentSectionData;
            }
            else if (!string.IsNullOrEmpty(trimmedLine) && currentSection != null)
            {
                int index = trimmedLine.IndexOf('=');
                if (index != -1)
                {
                    string key = trimmedLine.Substring(0, index).Trim();
                    string value = trimmedLine.Substring(index + 1).Trim();
                    currentSectionData[key] = value;
                }
            }
        }

        return iniData;
    }

    /// <summary>
    /// 将指定的 INI 文件内容写入到指定路径的文件中
    /// </summary>
    /// <param name="filePath">INI 文件路径</param>
    /// <param name="data">要写入的 INI 文件内容</param>
    public static void Write(string filePath, Dictionary<string, Dictionary<string, string>> data)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var section in data)
            {
                writer.WriteLine($"[{section.Key}]");
                foreach (var key in section.Value)
                {
                    writer.WriteLine($"{key.Key}={key.Value}");
                }
            }
        }
    }

    /// <summary>
    /// 修改 INI 文件中指定节和键的值
    /// </summary>
    /// <param name="filePath">INI 文件路径</param>
    /// <param name="sectionName">要修改的节名称</param>
    /// <param name="keyName">要修改的键名称</param>
    /// <param name="newValue">新的键值</param>
    public static void ModifyKey(string filePath, string sectionName, string keyName, string newValue)
    {
        var iniData = Read(filePath);

        if (iniData.ContainsKey(sectionName) && iniData[sectionName].ContainsKey(keyName))
        {
            iniData[sectionName][keyName] = newValue;
            Write(filePath, iniData);
            Console.WriteLine($"键 '{keyName}' 的值已修改为 '{newValue}'");
        }
        else
        {
            Console.WriteLine($"键 '{keyName}' 不存在于 '{sectionName}' 中");
        }
    }

    /// <summary>
    /// 从 INI 文件中获取指定节和键的值
    /// </summary>
    /// <param name="filePath">INI 文件路径</param>
    /// <param name="sectionName">节名称</param>
    /// <param name="keyName">键名称</param>
    /// <returns>键对应的值，如果键不存在则返回 null</returns>
    public static string GetValueByKey(string filePath, string sectionName, string keyName)
    {
        var iniData = Read(filePath);

        if (iniData.ContainsKey(sectionName) && iniData[sectionName].ContainsKey(keyName))
        {
            return iniData[sectionName][keyName];
        }
        else
        {
            Console.WriteLine($"键 '{keyName}' 不存在于 '{sectionName}' 中");
            return null;
        }
    }
}