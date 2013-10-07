#pragma once

#include <list>
#include "BlockBase.h"

namespace ScriptLibrary
{
    class Script
    {
    private:
        std::string m_name;
        std::list<BlockBase*> m_scriptBlocks;

    public:
        Script(const char* name);
        ~Script();

        void AppendBlockList(std::list<BlockBase*>& list);

        inline std::string& GetName() { return m_name; }
        inline const std::list<BlockBase*>* GetBlockList() { return &m_scriptBlocks; }
    };
}