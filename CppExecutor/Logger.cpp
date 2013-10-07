#include "Logger.h"
#include <iostream>

using namespace ScriptLibrary;

void Logger::Trace(std::string message)
{
    Log("Trace", message);
}

void Logger::Debug(std::string message)
{
    Log("Debug", message);
}

void Logger::Info(std::string message)
{
    Log("Info", message);
}

void Logger::Warning(std::string message)
{
    Log("Warning", message);
}

void Logger::Error(std::string message)
{
    Log("Error", message);
}

void Logger::Fatal(std::string message)
{
    Log("Fatal", message);
}

void Logger::Log(std::string cat, std::string& data)
{
    std::cout << "[" << cat << "] " << data << std::endl;
}