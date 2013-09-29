#pragma once
#include "IJsonSerializable.h"

// [GenerateClass(Namespace="Test.Smile")]
class TestStructure : public IJsonSerializable
{
public:
    // [GenerateProperty(Type="string", Display="Name", Show)]
    std::string m_Name;

    // [GenerateProperty(Type="int", Display="Id", Show)]
    int m_Id;

public:
    virtual ~TestStructure(void);

    virtual void Serialize(Json::Value& jsonData);

    virtual void Deserialize(Json::Value& jsonData);
};