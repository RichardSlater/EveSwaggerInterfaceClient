// ©2017 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

using System;
using EveSwaggerInterfaceClient.ComponentModel;
using EveSwaggerInterfaceClient.Properties;

namespace EveSwaggerInterfaceClient {
    public class AppConfigConfiguration : IConfigurationService {
        public T Get<T>(string key) {
            return (T) Convert.ChangeType(Settings.Default[key], typeof(T));
//            return ConfigurationManager.AppSettings.Get(key);
        }
    }
}