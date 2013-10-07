#include "StandardBlocks.h"
#include "Logger.h"
#include "DataStorage.h"

using namespace ScriptLibrary;
using namespace ScriptLibrary::StandardBlocks;

// ForLoop Block
ForLoop::ForLoop()
    : BlockBase("ForLoop")
{
    m_iterations = 0;
    m_currentIteration = 0;
}

void ForLoop::Run()
{
}

void ForLoop::Verify(double tickTime, double totalTime)
{
    ++m_currentIteration;
    if (RunChildren())
    {
        return;
    }

    SetComplete(Success);
}

// Log Block
Log::Log()
    : BlockBase("Log")
{
}

void Log::Run()
{
    Logger::Debug(m_message);
}

// Wait Block
Wait::Wait()
    : BlockBase("Wait")
{
}

void Wait::Run()
{
}

void Wait::Verify(double tickTime, double totalTime)
{
    if(totalTime >= m_waitTime)
    {
        SetComplete(Success);
    }
}