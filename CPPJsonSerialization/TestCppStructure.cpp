#include "TestCppStructure.h"

TestCppStructure::TestCppStructure(void)
{
    m_pListOfInts = new std::vector<int>();
}

TestCppStructure::~TestCppStructure(void)
{
    delete m_pListOfInts;
}

void TestCppStructure::Serialize(Json::Value& jsonData)
{
    jsonData["$type"] = "CSJsonSerialization.TestCppStructure, CSJsonSerialization";
    jsonData["Name"] = m_Name;
    jsonData["Id"] = m_Id;

    Json::Value listOfInts = Json::Value();
    listOfInts["$type"] = "System.Collections.Generic.List`1[[System.Int32, mscorlib]], mscorlib";

    for(std::vector<int>::iterator iter = m_pListOfInts ->begin(); iter != m_pListOfInts->end(); ++iter)
    {
        listOfInts["$values"].append((*iter));
    }

    jsonData["ListOfInts"] = listOfInts;
}

void TestCppStructure::Deserialize(Json::Value& jsonData)
{
    m_Name = jsonData.get("Name", "").asString();
    m_Id = jsonData.get("Id", 0).asInt();

    Json::Value listOfInts = jsonData.get("ListOfInts", NULL);
    
    if (listOfInts != NULL)
    {
        Json::Value listValues = listOfInts.get("$values", NULL);

        if(listValues != NULL)
        {
            m_pListOfInts->reserve(listValues.size());
            for(Json::Value::iterator iter = listValues.begin(); iter != listValues.end(); ++iter)
            {
                m_pListOfInts->push_back((*iter).asInt());
            }
        }
    }
}