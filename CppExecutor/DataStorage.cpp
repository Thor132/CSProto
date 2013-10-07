#include "DataStorage.h"

using namespace ScriptLibrary;

DataStorage::DataStorage()
{
};

DataStorage& DataStorage::GetInstance()
{
    static DataStorage instance; 
    return instance;
}

DataStorage::~DataStorage()
{
    for(std::map<std::string, void*>::iterator iter = m_dataStorage.begin(); iter != m_dataStorage.end(); ++iter)
    {
        delete (iter->second);
    }
}

void DataStorage::Set(std::string key, void* value)
{
    m_dataStorage[key] = value;
}

bool DataStorage::HasKey(std::string key)
{
    return m_dataStorage.count(key) != 0;
}