#include "JsonSerialization.h"
#include "TestStructure.h"
#include "TestCppStructure.h"

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

    
	std::string readGenFile = argv[1] + std::string("\\cppgen_csjson.txt");
    TestCppStructure* pTestCppStructure = new TestCppStructure(); 
	JsonSerialization::DeserializeObjectFromFile(pTestCppStructure, readGenFile.c_str());

    std::string writeGenFile = argv[1] + std::string("\\cppgen_cppjson.txt");
    JsonSerialization::SerializeObjectToFile(pTestCppStructure, writeGenFile.c_str());

	return 0;
}