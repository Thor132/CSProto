#pragma once

#include <string>
#include <map>

namespace ScriptLibrary
{
    union ValueTypeUnion
    {
        char          ch;
        int           i;
        unsigned int  ui;
        long          l;
        unsigned long ul;
        float         f;
        double        d;
    };

    class DataStorage
    {
    private:
        std::map<std::string, void*> m_dataStorage;

    public:
        DataStorage::~DataStorage();

        static DataStorage& GetInstance();

        void Set(std::string key, void* value);
        bool HasKey(std::string key);

        template<typename T>
        T Get(std::string key)          
        {
            if (!HasKey(key))
            {
                return NULL;
            }

            return static_cast<T>(m_dataStorage[key]);
        }

    private:
        DataStorage();
        DataStorage(DataStorage const&);
        void operator=(DataStorage const&);
    };
}