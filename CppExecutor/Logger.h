#pragma once

#include <string>

namespace ScriptLibrary
{
    class Logger
    {
    public:
        static void Trace(std::string message);
        static void Debug(std::string message);
        static void Info(std::string message);
        static void Warning(std::string message);
        static void Error(std::string message);
        static void Fatal(std::string message);

    protected:
        static void Log(std::string cat, std::string& data);
    };
}