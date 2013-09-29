#pragma once
#include <json/json.h>

class IJsonSerializable
{
public:
	virtual ~IJsonSerializable( void ) {};
	virtual void Serialize( Json::Value& jsonData ) = 0;
	virtual void Deserialize( Json::Value& jsonData) = 0;
};