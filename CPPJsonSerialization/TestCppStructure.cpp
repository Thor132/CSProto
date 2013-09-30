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
    //jsonData["$type"] = "CSJsonSerialization.TestStructure, CSJsonSerialization";
    jsonData["m_Name"] = m_Name;
    jsonData["m_Id"] = m_Id;
}

void TestCppStructure::Deserialize(Json::Value& jsonData)
{
    m_Name = jsonData.get("Name", "").asString();
    m_Id = jsonData.get("Id", 0).asInt();
}