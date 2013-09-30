#pragma once
#include "IJsonSerializable.h"

class TestStructure : public IJsonSerializable
{
public:
    std::string m_Name;
    int m_Id;

public:
    virtual ~TestStructure(void);

    virtual void Serialize(Json::Value& jsonData);

    virtual void Deserialize(Json::Value& jsonData);
};