// ©2017 Amido Limited (https://www.amido.com), Licensed under the terms of the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)

namespace EveSwaggerInterfaceClient.ComponentModel {
    public interface IConfigurationService {
        T Get<T>(string key);
    }
}