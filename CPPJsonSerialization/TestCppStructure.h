#pragma once
#include "IJsonSerializable.h"

// [GenerateClass(Name="TestCppStructure")]
class TestCppStructure : public IJsonSerializable
{
public:
    // [GenerateProperty(Name="Name", Type="string", DisplayName="Name")]
    std::string m_Name;

    // [GenerateProperty(Name="Id", Type="int", DisplayName="Id", Description="The structure's id")]
    int m_Id;

    // [GenerateProperty(Name="ListOfInts", Type="List<int>", Complex)]
    std::vector<int>* m_pListOfInts;

public:
    TestCppStructure(void);
    virtual ~TestCppStructure(void);

    virtual void Serialize(Json::Value& jsonData);

    virtual void Deserialize(Json::Value& jsonData);
};