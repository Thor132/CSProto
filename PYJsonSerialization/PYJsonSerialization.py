import json
import re
import ctypes.wintypes

# [GenerateClass(Name="TestPythonStructure")]
class TestPythonStructure(object):
    # [GenerateProperty(Name="Name", Type="string", DisplayName="Name")]
    # [GenerateProperty(Name="Id", Type="int", DisplayName="Id", Description="The structure's id")]
    # [GenerateProperty(Name="ListOfStrings", Type="List<string>", Complex)]
    def __init__(self, Name = "", Id = 0, ListOfInts = []):
        self.Name = Name
        self.Id = Id

class TestStructure(object):
    def __init__(self, Name = "", Id = 0):
        self.Name = Name
        self.Id = Id

    def PrintData(self):
        print "Name=\"" + self.Name + "\" Id=" + str(self.Id)

class JsonSerialization(object):
    def SerializeObjectToString(self, object):
        return json.dumps(object, indent=2, default=self.convert_to_builtin_type)

    def SerializeObjectToFile(self, object, filename):
        file = open(filename, "w")
        file.write(self.SerializeObjectToString(object))
        file.close()

    def DeserializeObjectFromString(self, data):
       return json.loads(data, object_hook=self.dict_to_object)

    def DeserializeObjectFromFile(self, filename):
        file = open(filename, "r")
        data = file.read()
        file.close()
        return self.DeserializeObjectFromString(data)

    def convert_to_builtin_type(self, obj):
        print 'default(', repr(obj), ')'
        # Convert objects to a dictionary of their representation
        d = { '__class__':obj.__class__.__name__, '__module__':obj.__module__,}
        d.update(obj.__dict__)
        return d

    def dict_to_object(self, d):
        if '__class__' in d:
            class_name = d.pop('__class__')
            module_name = d.pop('__module__')
            module = __import__(module_name)
            print 'MODULE:', module
            class_ = getattr(module, class_name)
            print 'CLASS:', class_
            args = dict( (key.encode('ascii'), value) for key, value in d.items())
            print 'INSTANCE ARGS:', args
            inst = class_(**args)
        else:
            inst = d
        return inst

#Inherited serialization
class JsonSerializationCSharp(JsonSerialization):
    def __init__(self, namespace = "", assembly = "", module = "__main__"):
        self._csNamespace = namespace
        self._csAssemblyName = assembly
        self._moduleName = module

    def convert_to_builtin_type(self, obj):
        print 'default(', repr(obj), ')'
        # Add type information for the C# json deserializer
        csTypeInfo = self._csNamespace + "." + obj.__class__.__name__ + ", " + self._csAssemblyName
        d = { '$type': csTypeInfo}
        d.update(obj.__dict__)
        return d

    def dict_to_object(self, d):
        if '$type' in d:
            typeString = d.pop('$type')
            reMatch = re.match("(.+?), .*", typeString)
            if not reMatch:
                return None

            module = __import__(self._moduleName)
            print 'MODULE:', module
            className = str(reMatch.group(1)).split(".").pop()
            class_ = getattr(module, className)
            print 'CLASS:', class_
            args = dict( (key.encode('ascii'), value) for key, value in d.items())
            print 'INSTANCE ARGS:', args
            inst = class_(**args)
        else:
            inst = d
        return inst

def GetDirectory(id):
    buf = ctypes.create_unicode_buffer(ctypes.wintypes.MAX_PATH)
    ctypes.windll.shell32.SHGetFolderPathW(0, id, 0, 0, buf)
    return buf.value

if __name__ == "__main__":
    csharpInputFile = GetDirectory(0) + "\\csjson.txt"
    pythonOutputFile = GetDirectory(0) + "\\pyjson.txt"

    pyTestStructure = TestStructure("Python Test", 12)
    pyTestStructure.PrintData()

    serializer = JsonSerializationCSharp("CSJsonSerialization", "CSJsonSerialization")

    # Output file for C# to read
    serializer.SerializeObjectToFile(pyTestStructure, pythonOutputFile)

    # Read C# File
    csTestStructure = serializer.DeserializeObjectFromFile(csharpInputFile)
    csTestStructure.PrintData()