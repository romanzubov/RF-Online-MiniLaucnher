using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MiniLauncher.Utils
{
    public class UserCredential
    {
        private string pwd = "UGJhhQxR9h7EwCb7B*SpXDe?N@+?Be*kke2GXfMXmnErVtmMckgZEm##PaFufUDN5r?&YUP^PELEG7?XZ76jQg-n9XWWW+Xt$R8BW%rx6N%f*NC-%Z@S$dqd+_ZH";
        private readonly string _dataFile;
        private Dictionary<string, string> DecryptedAuthenticationData { get; set; }
        public UserCredential(string dataFile)
        {
            _dataFile = dataFile;
            LoadFile();
        }
        public string[] LoadLogins()
        {
            return DecryptedAuthenticationData.Keys.ToArray();
        }
        public string ProcessLoginData(string login, string password = null)
        {
            password = String.IsNullOrEmpty(password) ? String.Empty : password;
            if (!DecryptedAuthenticationData.ContainsKey(login))
            {
                // Add
                VaultAdd(new KeyValuePair<string, string>(login, password));
                return password;
            }

            if (DecryptedAuthenticationData.TryGetValue(login, out string password_from_file))
            {
                if(password != String.Empty && password_from_file != password)
                {
                    // Update
                    VaultUpdate(new KeyValuePair<string, string>(login, password));
                    return password;
                }
                return password_from_file;
            }
            return String.Empty;
        }
        private void VaultAdd(KeyValuePair<string,string> loginPass)
        {
            DecryptedAuthenticationData.Add(loginPass.Key, loginPass.Value);
            Save();
        }
        private void VaultUpdate(KeyValuePair<string, string> loginPass)
        {
            DecryptedAuthenticationData[loginPass.Key] = loginPass.Value;
            Save();
        }
        private void Save()
        {
            var CryptedAuthenticationData = new Dictionary<string, string>();
            foreach( var KeyValuePair in DecryptedAuthenticationData)
            {
                CryptedAuthenticationData.Add(StringCipher.Encrypt(KeyValuePair.Key,pwd),
                    StringCipher.Encrypt(KeyValuePair.Value, pwd));
            }
            using (StreamWriter file = File.CreateText(_dataFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, CryptedAuthenticationData);
            }
        }
        public static void CleanData(string file_name)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            if (File.Exists(file_name))
            {
                using (StreamWriter file = File.CreateText(file_name))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, data);
                }
            }
        }
        private void LoadFile()
        {
            DecryptedAuthenticationData = new Dictionary<string, string>();
            if (!File.Exists(_dataFile))
            {
                using (StreamWriter file = File.CreateText(_dataFile))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, DecryptedAuthenticationData);
                }
            }
            else
            {
                using (StreamReader file = File.OpenText(_dataFile))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var CryptedAuthenticationData = (Dictionary<string, string>)serializer.Deserialize(file, typeof(Dictionary<string, string>));
                    foreach(var keyValuePair in CryptedAuthenticationData)
                    {
                        try
                        {
                            DecryptedAuthenticationData[StringCipher.Decrypt(keyValuePair.Key, pwd)] =
                                StringCipher.Decrypt(keyValuePair.Value, pwd);
                        }
                        catch (Exception e)
                        {
                            // Запись в старом формате (Rijndael/256) или повреждена — пропускаем.
                            SimpleLogger.GetInstance.Warning("Credential entry skipped: " + e.Message);
                        }
                    }
                }
            }
        }
    }
}
