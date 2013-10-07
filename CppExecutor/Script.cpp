#include "Script.h"

using namespace ScriptLibrary;

Script::Script(const char* name)
    : m_name(name)
{
}

Script::~Script()
{
    for (std::list<BlockBase*>::iterator iter = m_scriptBlocks.begin(); iter != m_scriptBlocks.end(); ++iter)
    {
        delete (*iter);
    }
}

void Script::AppendBlockList(std::list<BlockBase*>& list)
{
    m_scriptBlocks.splice(m_scriptBlocks.end(), list);
}