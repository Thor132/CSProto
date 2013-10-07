#pragma once

#include "Script.h"
#include <ctime> 

namespace ScriptLibrary
{
    // TODO: use chrono for timing
    class Executor
    {
    private:
        bool m_finished;
        bool m_passed;

    public:
        bool RunScript(Script* pScript);

    protected:
        void RunBlock(const std::list<BlockBase*>* pBlockList, bool forceExecution = false);
        bool IsBlockRequired(const BlockBase* pBlock);
        void Tick(std::time_t& lastTime, double& tickTime);
    };
}