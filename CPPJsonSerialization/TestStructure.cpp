#include "TestStructure.h"

TestStructure::~TestStructure(void)
{
}

void TestStructure::Serialize(Json::Value& jsonData)
{
    jsonData["$type"] = "CSJsonSerialization.TestStructure, CSJsonSerialization";
    jsonData["Name"] = m_Name;
    jsonData["Id"] = m_Id;
}

void TestStructure::Deserialize(Json::Value& jsonData)
{
    m_Name = jsonData.get("Name", "").asString();
    m_Id = jsonData.get("Id", 0).asInt();
}