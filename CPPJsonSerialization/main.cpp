#include "JsonSerialization.h"

class TestStructure : public IJsonSerializable
{
public:
	std::string m_Name;
	int m_Id;

public:
	virtual ~TestStructure(void)
	{
	}

	virtual void Serialize(Json::Value& jsonData)
	{
		jsonData["$type"] = "CSJsonSerialization.TestStructure, CSJsonSerialization";
		jsonData["Name"] = m_Name;
		jsonData["Id"] = m_Id;
	}

	virtual void Deserialize(Json::Value& jsonData)
	{
		m_Name = jsonData.get("Name", "").asString();
		m_Id = jsonData.get("Id", 0).asInt();
	}
};

int main(int argc, const char *argv[])
{
	if (argc < 2)
	{
		std::cout << "Need to pass in target directory for the files." << std::endl;
		return 1;
	}

	std::string writeFile = argv[1] + std::string("\\cppjson.txt");
	std::string readFile = argv[1] + std::string("\\csjson.txt");

	TestStructure* pTestStructure = new TestStructure();
	pTestStructure->m_Name = "CPP name";
	pTestStructure->m_Id = 99;

	JsonSerialization::SerializeObjectToFile(pTestStructure, writeFile.c_str());

	TestStructure* pCSTestStructure = new TestStructure(); 
	JsonSerialization::DeserializeObjectFromFile(pCSTestStructure, readFile.c_str());
	return 0;
}