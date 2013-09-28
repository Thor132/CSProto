#include <fstream>
#include "IJsonSerializable.h"

class JsonSerialization
{
public:
	static bool SerializeObjectToString(IJsonSerializable* pObj, std::string& output);

	static bool SerializeObjectToFile(IJsonSerializable* pObj, const char* filename);

	static bool DeserializeObjectFromString(IJsonSerializable* pObj, std::string& input);

	static bool DeserializeObjectFromFile(IJsonSerializable* pObj, const char* filename);

private:
	static std::string ReadFileToString(const char* filename);
};