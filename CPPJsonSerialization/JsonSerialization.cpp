#include "JsonSerialization.h"

bool JsonSerialization::SerializeObjectToString(IJsonSerializable* pObj, std::string& output)
{
	if (pObj == NULL)
		return false;

	Json::Value serializeRoot;
	pObj->Serialize(serializeRoot);

	Json::StyledWriter writer;
	output = writer.write(serializeRoot);

	return true;
}

bool JsonSerialization::SerializeObjectToFile(IJsonSerializable* pObj, const char* filename)
{
	std::string data;
	if (SerializeObjectToString(pObj, data))
	{
		std::ofstream file(filename, std::ios::out);
		file.write(data.c_str(), data.size());
		file.close();
		return true;
	}

	return false;
}

bool JsonSerialization::DeserializeObjectFromString(IJsonSerializable* pObj, std::string& input)
{
	if (pObj == NULL)
	{
		return false;
	}

	Json::Value deserializedJson;
	Json::Reader jsonReader;

	if (!jsonReader.parse(input, deserializedJson))
	{
		pObj = NULL;
		return false;
	}

	pObj->Deserialize(deserializedJson);

	return true;
}

bool JsonSerialization::DeserializeObjectFromFile(IJsonSerializable* pObj, const char* filename)
{
	std::string data = ReadFileToString(filename);
	return DeserializeObjectFromString(pObj, data);
}

std::string JsonSerialization::ReadFileToString(const char* filename)
{
	std::ifstream file(filename, std::ios::in);
	if (file)
	{
		std::string contents;
		file.seekg(0, std::ios::end);
		contents.resize((unsigned int)file.tellg());
		file.seekg(0, std::ios::beg);
		file.read(&contents[0], contents.size());
		file.close();
		return(contents);
	}

	return "";
}